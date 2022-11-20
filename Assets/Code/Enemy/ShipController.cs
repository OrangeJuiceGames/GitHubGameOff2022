using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MobSpawner))]
public class ShipController : MonoBehaviour
{
    public event Action<ShipController> OnDestroyed;
    public event Action<ShipController> OnActivated;

    public MobSpawner MobSpwaner => _MobSpawner;
    public bool IsActive { get; set; }

    [SerializeField] SpriteRenderer _model;
    private MobSpawner _MobSpawner;
    private ShipMovement _shipMovement;
    private bool _IsActive;

    private void Awake()
    {
        _MobSpawner = GetComponent<MobSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _shipMovement = new ShipMovement(this.transform.position, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_IsActive)
        {
            return;
        }
        transform.position = _shipMovement.GetNewPosition();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "RightBlocker" || collision.gameObject.name == "LeftBlocker")
        {
            _shipMovement.ChangeDirection();
        }
    }

}
