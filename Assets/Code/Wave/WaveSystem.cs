using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : Updatable
{
    public event Action<int> OnWaveStarted;
    public event Action OnRestComplete;
    
    /*
    public void StartWave(List<ShipController> ships)
    {
        _SpinControllers = ships;
        _WavePhase.StateChange(WavePhase.StartOfWave);
        Register();
    }
    */

    public void Init()
    {
        _Wave = 0;
        _SpinControllers = new List<ShipController>();
        var ship = _Stage.ShipFactory.BuidShip();
        ship.SetActive(false);
        _SpinControllers.Add(ship);
        Register();

        _WavePhase.StateChange(WavePhase.Rest);
    }

    public WaveSystem(Stage stage)
    {
        _Stage = stage;
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
    private Stage _Stage;

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
        _WavePhase = new StateActionMap<WavePhase>();
        _WavePhase.RegisterEnter(WavePhase.Rest, OnEnter_Rest);
        _RestTimer.OnTimerComplete += OnRestTimerComplete;

        _WavePhase.RegisterEnter(WavePhase.StartOfWave, OnEnter_StartOfWave);
        _WavePhase.RegisterEnter(WavePhase.Spawning, OnEnter_Spawning);

        _WavePhase.RegisterEnter(WavePhase.ShipsDestroyed, OnEnter_ShipsDestroyed);
        _StartBossTimer.OnTimerComplete += OnStartBossTimerComplete;

        _WavePhase.RegisterEnter(WavePhase.Boss, OnEnter_Boss);
    }

    private Timer _RestTimer = new Timer();
    private float _RestTime = 1000f;
    private void OnEnter_Rest()
    {
        _RestTimer.Start(_RestTime);
    }

    private void OnRestTimerComplete()
    {
        Debug.Log("OnRestComplete");
        _WavePhase.StateChange(WavePhase.StartOfWave);
        OnRestComplete?.Invoke();
    }

    private void OnEnter_StartOfWave()
    {
        _Wave++;

        for(int i = 0; i < _SpinControllers.Count; i++)
        {
            _SpinControllers[i].SetActive(true);
        }

        _WavePhase.StateChange(WavePhase.Spawning);
        OnWaveStarted?.Invoke(_Wave);
    }        

    private void OnEnter_Spawning()
    {
        for (int i = 0; i < _SpinControllers.Count; i++)
        {
            _SpinControllers[i].SetSpawning(true);
        }
    }

    private Timer _StartBossTimer = new Timer();
    private float _WaitTimeForBoss = 1000f;
    private void OnEnter_ShipsDestroyed()
    {
        _StartBossTimer.Start(_WaitTimeForBoss);
    }

    private void OnStartBossTimerComplete()
    {
        _WavePhase.StateChange(WavePhase.Boss);
    }

    private void OnEnter_Boss()
    {
        Debug.Log("Boss Phase");
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
