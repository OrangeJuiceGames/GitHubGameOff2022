using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{   
    public event Action OnDestroyed;

    [SerializeField]
    private Animator _Effect, _Animator;
    [SerializeField]
    private MobSpawner _MobSpawnerL, _MobSpawnerR;

    public void Destroy()
    {
        _IsSpawning = false;
        OnDestroyed?.Invoke();
    }

    public void Activate(int wave)
    {
        _Effect.Play("None");
        _healthController.UpdateHealth(128 * wave);
        gameObject.SetActive(true);
        _IsSpawning = true;
        _SpawnTimer.Start(666);
    }

    private HealthController _healthController;
    private Instantiation _DeathExplosion;
    private bool _IsSpawning;
    private Timer _SpawnTimer = new Timer();
    private WTMK _Tools = WTMK.Instance;

    private void Awake()
    {
        _DeathExplosion = GetComponent<Instantiation>();
        _DeathExplosion.OnExplosionComplete += DeathExplosionComplete;

        _healthController = new HealthController(100, .60f, .30f);

        _healthController.OnReachZeroHealth += DestroyShip;
        _healthController.OnReachCriticalHealth += ShowCriticalHealthAnim;
        _healthController.OnReachLowHealth += ShowLowHealthAnim;

        _SpawnTimer.OnTimerComplete += SpawnTimerComeplete;
    }

    private void Update()
    {
        _SpawnTimer.Update();
    }

    private void SpawnTimerComeplete()
    {
        if(_IsSpawning)
        {
            var timeTillNextSpawn = _Tools.Rando.Next(1200, 3200);
            var left = _Tools.Rando.Next(10);

            if(left > 4)
            {
                _MobSpawnerL.SpawnMob(_MobSpawnerL.transform.position, MobType.Dog);
            }
            else
            {
                _MobSpawnerR.SpawnMob(_MobSpawnerR.transform.position, MobType.Dog);
            }

            _SpawnTimer.Start(timeTillNextSpawn);
        }
    }

    private void DestroyShip()
    {
        _DeathExplosion.DoExplode();
        AudioManager.Instance.PlayAudioByEnumType(AudioType.ShipDestroyed);
        OnDestroyed?.Invoke();
    }

    private void DeathExplosionComplete()
    {
        //SetActive(false);
    }

    private void ShowLowHealthAnim()
    {
        _Animator.SetTrigger("Low");
    }

    private void ShowCriticalHealthAnim()
    {
        _Animator.SetTrigger("Crit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                var shot = collision.gameObject.GetComponent<Shot>();
                HandelShotCollision(shot);
                break;
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        //deal damage to ship 
        _Effect.transform.position = shot.transform.position;
        _Effect.SetTrigger("Hit");
        _healthController.DamageHealth(shot.Damage);
    }
}
