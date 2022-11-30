using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MobSpawner))]
public class ShipController : MonoBehaviour
{
    public event Action<ShipController> OnDestroyed;

    public MobSpawner MobSpwaner => _MobSpawner;
    public bool IsActive => _IsActive;

    public void Destory()
    {
        _IsActive = false;
        _MobSpawner.SetSpawning(false);
        
    }

    public void SetActive(bool isActive)
    {
        _IsActive = isActive;
        _Animator.SetTrigger("Reset");
        _healthController.ResetHealth();
        gameObject.SetActive(isActive);
    }

    public void SetSpawning(bool isActive)
    {
        _MobSpawner.SetSpawning(isActive);
    }

    public void SetShipStats(float newMovementSpeed, float newStopLength)
    {
        _shipMovement.UpdateMoveSpeed(newMovementSpeed);
        _shipMovement.UpdateStopLength(newStopLength);
    }

    public void DoUpdate()
    {
        if (!_IsActive)
        {
            return;
        }

        transform.position = _shipMovement.MoveTowardsStop();
    }


    [SerializeField] SpriteRenderer _model;
    [SerializeField] Animator _EffectAnimator, _Animator;
    private MobSpawner _MobSpawner;
    private ShipMovement _shipMovement;
    private HealthController _healthController;
    private bool _IsActive, _IsDestroyed;
    private Instantiation _DeathExplosion;
    private float _DestroyedTimer = 3f;

    private void Awake()
    {
        _DeathExplosion = GetComponent<Instantiation>();
        _DeathExplosion.OnExplosionComplete += DeathExplosionComplete;
        _MobSpawner = GetComponent<MobSpawner>();

        _shipMovement = new ShipMovement(1.25f, 0.5f, transform.position, Paths);
        _healthController = new HealthController(100, .60f, .30f);

        _healthController.OnReachZeroHealth += DestroyShip;
        _healthController.OnReachCriticalHealth += ShowCriticalHealthAnim;
        _healthController.OnReachLowHealth += ShowLowHealthAnim;
    }

    [SerializeField] List<ShipPath> Paths;

    // Start is called before the first frame update
    void Start()
    {
        _shipMovement.OnReachStop += _MobSpawner.SpawnMob;
        _MobSpawner.OnSpawnComplete += _shipMovement.SpawnComplete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                var shot = collision.gameObject.GetComponent<Shot>();
                HandelShotCollision(shot);
                break;
            case "Floor":
                
                break;
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        //deal damage to ship 
        _EffectAnimator.SetTrigger("Hit");
        _healthController.DamageHealth(shot.Damage);
        AudioManager.Instance.PlayAudioByEnumType( AudioType.ShipLightDamageTaken );
    }

    private void DestroyShip()
    {
        _DeathExplosion.DoExplode();
        AudioManager.Instance.PlayAudioByEnumType( AudioType.ShipDestroyed );
        OnDestroyed?.Invoke(this);
    }

    private void DeathExplosionComplete()
    {
        SetActive(false);
    }

    private void ShowLowHealthAnim()
    {
        _Animator.SetTrigger("Low");
    }

    private void ShowCriticalHealthAnim()
    {
        _Animator.SetTrigger("Crit");
    }
}
