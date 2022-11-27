using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : Updatable
{
    public event Action<int> OnWaveStarted;
    public event Action OnRestComplete;

    public void Init()
    {
        _Wave = 0;
        _ShipsActive = 1;
        _WavePhase.StateChange(WavePhase.Rest);
    }

    public WaveSystem(Stage stage)
    {
        _Stage = stage;
        BuildWavePhase();
        BuildShipControllers();
        Register();
    }

    public void Update()
    {
        UpdateTimer();
    }

    private static readonly int SHIPS_ACTIVE_MAX = 3;

    private int _Wave, _ShipsActive;
    private int _ShipsSpawned;
    private float _InvasionTime;
    private List<ShipController> _ShipControllers;
    private StateActionMap<WavePhase> _WavePhase;
    private Stage _Stage;

    private void Register()
    {
        for(int i = 0; i < _ShipControllers.Count; i++)
        {
            _ShipControllers[i].OnDestroyed += ShipDestroyed;
        }

        _Stage.Boss.OnDestroyed += BossDestroyed;
        _Stage.Boss.gameObject.SetActive(false);
    }

    private Timer _ShipSpawnTimer = new Timer();
    private float _SpawnDelay = 1000f;

    private void ActivateShips()
    {
        _ShipControllers[_ShipsSpawned].SetActive(true);
        _ShipsSpawned++;

        if (_ShipsSpawned < _ShipsActive)
        {
            _ShipSpawnTimer.Start(_SpawnDelay);
        }
    }

    private void ShipDestroyed(ShipController ship)
    {
        if(_WavePhase.CurrentState == WavePhase.Spawning)
        {
            _ShipsActive--;
            if (_ShipsActive <= 0)
            {
                _WavePhase.StateChange(WavePhase.ShipsDestroyed);
            }
        }
    }

    private void BuildShipControllers()
    {
        _ShipControllers = new List<ShipController>();

        var count = 0;
        while(count < SHIPS_ACTIVE_MAX)
        {    
            var ship = _Stage.ShipFactory.BuidShip(_Wave);
            ship.SetActive(false);
            _ShipControllers.Add(ship);
            count++;
        }
    }

    private void UpdateTimer()
    {
        _RestTimer.Update();
        _StartBossTimer.Update();
        _ShipSpawnTimer.Update();
    }

    private void BuildWavePhase()
    {
        _WavePhase = new StateActionMap<WavePhase>();
        _WavePhase.RegisterEnter(WavePhase.Rest, OnEnter_Rest);
        _RestTimer.OnTimerComplete += OnRestTimerComplete;

        _WavePhase.RegisterEnter(WavePhase.StartOfWave, OnEnter_StartOfWave);
        _ShipSpawnTimer.OnTimerComplete += ActivateShips;
        _WavePhase.RegisterEnter(WavePhase.Spawning, OnEnter_Spawning);

        _WavePhase.RegisterEnter(WavePhase.ShipsDestroyed, OnEnter_ShipsDestroyed);
        _StartBossTimer.OnTimerComplete += OnStartBossTimerComplete;

        _WavePhase.RegisterEnter(WavePhase.Boss, OnEnter_Boss);

        _WavePhase.RegisterEnter(WavePhase.BossDestroyed, OnEnter_BossDestroyed);
        _BossDestroyedTimer.OnTimerComplete += OnBossDestroyedTimerComplete;
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
        OnRestComplete?.Invoke();

        _WavePhase.StateChange(WavePhase.StartOfWave);
    }

    private void OnEnter_StartOfWave()
    {
        _Wave++;
        //set number of active ships +1 for every 3 rounds to a max
        ActivateShips();

        _WavePhase.StateChange(WavePhase.Spawning);
        OnWaveStarted?.Invoke(_Wave);
    }        

    private void OnEnter_Spawning()
    {
        for (int i = 0; i < _ShipControllers.Count; i++)
        {
            _ShipControllers[i].SetSpawning(true);
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
        _Stage.Boss.gameObject.SetActive(true);
    }

    private void BossDestroyed()
    {
        _WavePhase.StateChange(WavePhase.BossDestroyed);
    }

    private Timer _BossDestroyedTimer = new Timer();
    private float _WaitTimeForRest = 1000f;
    private void OnEnter_BossDestroyed()
    {
        _Stage.Boss.gameObject.SetActive(false);
        _BossDestroyedTimer.Start(_WaitTimeForRest);
    }

    private void OnBossDestroyedTimerComplete()
    {
        _WavePhase.StateChange(WavePhase.Rest);
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
