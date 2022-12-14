using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : Updatable
{
    public event Action<int> OnWaveStarted;
    public event Action OnRestComplete;
    public event Action OnGameEnd;

    public void Init()
    {
        _Wave = 0;
        _ShipsSpawned = 0;
        _ShipsActive = 1;
        _InvasionSeconds = 0;
        _SecondsRounder = 1;

        DeactivateShips();

        var text = $"00:01";
        _Stage.InvasionTime.SetText(SpriteTextUtility.ConvertToTextImage(text));

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
        HandelInvasionTimer();
        UpdateShipControllers();
    }

    private static readonly int SHIPS_ACTIVE_MAX = 3;

    private int _Wave, _ShipsActive;
    private int _ShipsSpawned, _InvasionMin;
    private float _InvasionSeconds;
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

    private void UpdateShipControllers()
    {
        for (int i = 0; i < _ShipControllers.Count; i++)
        {
            if(_ShipControllers[i].IsActive)
            {
                _ShipControllers[i].DoUpdate();
            }
        }
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

    private void DeactivateShips()
    {
        for (int i = 0; i < _ShipControllers.Count; i++)
        {
            _ShipControllers[i].SetActive(false);
        }
    }

    private void DeactivateShip(ShipController ship)
    {
        ship.SetActive(false);
    }

    private void ShipDestroyed(ShipController ship)
    {
        ship.Destory();

        if (_WavePhase.CurrentState == WavePhase.Spawning)
        {
            _ShipsSpawned--;
            if (_ShipsSpawned <= 0)
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
        _BossDestroyedTimer.Update();
    }

    private int _SecondsRounder = 1, _OneSec = 60, _MaxWaveTimeInMin = 30;
    private void HandelInvasionTimer()
    {
        _InvasionSeconds += Time.deltaTime;

        if(_InvasionSeconds > _SecondsRounder)
        {
            _SecondsRounder++;

            if(_SecondsRounder > _OneSec)
            {
                _InvasionSeconds = 0f;
                _SecondsRounder = 1;
                _InvasionMin++;

                if(_InvasionMin == _MaxWaveTimeInMin)
                {
                    Debug.LogWarning("game over");
                    OnGameEnd?.Invoke();
                }
            }

            var text = "";

            if(_InvasionMin > 9)
            {
                text = $"{_InvasionMin}";
            }
            else
            {
                text = $"0{_InvasionMin}";
            }


            if(_SecondsRounder > 9)
            {
                text += $":{_SecondsRounder}";
            }
            else
            {
                text += $":0{_SecondsRounder}";
            }

            _Stage.InvasionTime.SetText(SpriteTextUtility.ConvertToTextImage(text));
        }
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
    private float _RestTime = 3000f;

    private void OnEnter_Rest()
    {
        _Stage.Wave.SetText($"Alive?"); //set random things to say
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
        _Stage.Wave.SetText($"WAVE\n{_Wave}");

        SetNumberOfActiveShips();
        ActivateShips();
        _WavePhase.StateChange(WavePhase.Spawning);
        OnWaveStarted?.Invoke(_Wave);
    }        

    private void SetNumberOfActiveShips()
    {
        if (_Wave > 9)
        {
            _ShipsActive = 3;
        }
        else if (_Wave > 6)
        {
            _ShipsActive = 2;
        }
        else
        {
            _ShipsActive = 1;
        }
    }

    private void OnEnter_Spawning()
    {
        for (int i = 0; i < _ShipControllers.Count; i++)
        {
            _ShipControllers[i].SetSpawning(true);
        }
    }

    private Timer _StartBossTimer = new Timer();
    private float _WaitTimeForBoss = 4200f;
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
        _Stage.Boss.Activate(_Wave);
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
