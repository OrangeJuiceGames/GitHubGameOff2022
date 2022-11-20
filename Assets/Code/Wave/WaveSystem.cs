using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : Updatable
{
    public event Action<int> OnWaveStarted;
    
    public void StartWave(List<ShipController> ships)
    {
        _SpinControllers = ships;
        Register();
    }

    public WaveSystem(MobSpawner mobSpwaner)
    {
        _MobSpwaner = mobSpwaner;
        BuildWavePhase();
    }

    public void Update()
    {
        UpdateTimer();
    }

    private int _Wave, _ShipsActive;
    private float _InvasionTime;
    private List<ShipController> _SpinControllers;
    private StateActionMap<WavePhase> _WavePhase;
    private MobSpawner _MobSpwaner;

    private void Register()
    {
        for(int i = 0; i < _SpinControllers.Count; i++)
        {
            _SpinControllers[i].OnActivated += ShipActivated;
            _SpinControllers[i].OnDestroyed += ShipDestroyed;
        }
    }

    private void Unregister()
    {
        for (int i = 0; i < _SpinControllers.Count; i++)
        {
            _SpinControllers[i].OnActivated -= ShipActivated;
            _SpinControllers[i].OnDestroyed -= ShipDestroyed;
        }
    }

    private void ShipActivated(ShipController ship)
    {
        _ShipsActive++;
    }

    private void ShipDestroyed(ShipController ship)
    {
        _ShipsActive--;
        if(_ShipsActive <= 0)
        {
            _WavePhase.StateChange(WavePhase.ShipsDestroyed);
        }
    }

    private void UpdateTimer()
    {
        _RestTimer.Update();
    }

    private void BuildWavePhase()
    {
        _Wave = 0;

        _WavePhase = new StateActionMap<WavePhase>();
        _WavePhase.RegisterEnter(WavePhase.Rest, OnEnter_Rest);
        _RestTimer.OnTimerComplete += OnRestComplete;

        _WavePhase.RegisterEnter(WavePhase.Spawning, OnEnter_Spawning);

        _WavePhase.RegisterEnter(WavePhase.ShipsDestroyed, OnEnter_ShipsDestroyed);

    }

    private Timer _RestTimer = new Timer();
    private float _RestTime = 1000;
    private void OnEnter_Rest()
    {
        _RestTimer.Start(_RestTime);
    }

    private void OnRestComplete()
    {
        _WavePhase.StateChange(WavePhase.StartOfWave);
    }

    private void OnEnter_StartOfWave()
    {
        _Wave++;
        _WavePhase.StateChange(WavePhase.Spawning);
        OnWaveStarted?.Invoke(_Wave);
    }        

    private void OnEnter_Spawning()
    {
        _MobSpwaner.StartSpawning(_Wave);
    }

    private void OnEnter_ShipsDestroyed()
    {

    }

}

public enum WavePhase
{
    Rest,
    StartOfWave,
    Spawning,
    ShipsDestroyed,
    StartOfBoss,
    Boss,
    BossDestroyed
}
