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

    public void SetActive(bool isActive)
    {
        _IsActive = isActive;
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
    private MobSpawner _MobSpawner;
    private ShipMovement _shipMovement;
    private HealthController _healthController;
    private bool _IsActive;

    private void Awake()
    {
        _MobSpawner = GetComponent<MobSpawner>();

        _shipMovement = new ShipMovement(1.25f, 0.5f, transform.position, Paths);
        _healthController = new HealthController(100, .30f, .10f);

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
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        //deal damage to ship 
        _healthController.DamageHealth(shot.Damage);
    }

    private void DestroyShip()
    {
        OnDestroyed?.Invoke(this);
    }

    private void ShowLowHealthAnim()
    {
        //trigger animation
    }

    private void ShowCriticalHealthAnim()
    {
        //trigger animation
    }
}
