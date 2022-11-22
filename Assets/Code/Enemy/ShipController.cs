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


    [SerializeField] SpriteRenderer _model;
    private MobSpawner _MobSpawner;
    private ShipMovement _shipMovement;
    private bool _IsActive;

    private void Awake()
    {
        _MobSpawner = GetComponent<MobSpawner>();
    }

    [SerializeField] List<ShipPath> Paths;

    // Start is called before the first frame update
    void Start()
    {
        _shipMovement = new ShipMovement(1.25f, 0.5f, transform.position, Paths, _MobSpawner);
        _shipMovement.OnReachStop += _MobSpawner.SpawnMob;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_IsActive)
        {
            return;
        }
        transform.position = _shipMovement.MoveTowardsStop();
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
        OnDestroyed?.Invoke(this);
    }
}
