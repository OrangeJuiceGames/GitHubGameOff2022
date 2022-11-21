using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    public void Spawn(MobType mobType, Vector3 pos)
    {
        _Rig.mass = 4.75f;
        _Rig.velocity = Vector3.zero;
        _Rig.angularVelocity = 0f;
        _Rig.gravityScale = 0.325f;
        transform.position = pos;
        ChangeMobType(mobType);
        SetActive(true);
    }

    public void Return()
    {
        _Rig.velocity = Vector2.zero;
        OnReturnRequest?.Invoke(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if (isActive)
        {
            _SkinMob.StateChange(_mobType);
        }
    }

    public MobType GetMobType()
    {
        return _mobType;
    }

    public void ChangeMobType(MobType newMobType)
    {
        _mobType = newMobType;
        transform.name = newMobType.ToString();
    }

    [SerializeField]
    private RuntimeAnimatorController _Dog, _Cat;
    [SerializeField]
    private Helmet _Helmet;
    [SerializeField]
    private Transform _Stage;
    [SerializeField]
    private float _CatReturnTime = 10f;

    private MobType _mobType = MobType.Cat;
    private Vector3 _impulseForce = new Vector3(0, 20f);
    private Vector3 _HelmetPosition;    
    private Rigidbody2D _Rig;
    private Animator _Animator;

    private StateActionMap<MobType> _SkinMob;
    private float _ReturnTimer = 10f;
    private bool _WillReturn;
    

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();

        _SkinMob = new StateActionMap<MobType>();
        _SkinMob.RegisterEnter(MobType.Cat, OnEnter_Cat);
        _SkinMob.RegisterEnter(MobType.CatWithHelmet, OnEnter_CatWithHelmet);
        _SkinMob.RegisterEnter(MobType.Dog, OnEnter_Dog);

        _Helmet.OnReturn += ReturnHelmet;

        _HelmetPosition = _Helmet.transform.position;
    }

    private void Update()
    {
        if(_WillReturn)
        {
            _ReturnTimer -= Time.deltaTime;
            if(_ReturnTimer <= 0)
            {
                _WillReturn = false;
                _ReturnTimer = _CatReturnTime;
                Return();
            }
        }
    }

    private void OnEnter_Cat()
    {
        _Animator.runtimeAnimatorController = _Cat;
        //remove helmet
    }

    private void OnEnter_CatWithHelmet()
    {
        _Animator.runtimeAnimatorController = _Cat;
        _Helmet.gameObject.SetActive(true);
        _Helmet.transform.SetParent(_Stage);
        //turn on helmet
    }

    private void OnEnter_Dog()
    {
        _Animator.runtimeAnimatorController = _Dog;
        _Helmet.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Mob collided with " + collision.gameObject.name);
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                AddUpwardsImpulse();
                break;
            case "Floor":
                var floor = collision.gameObject.GetComponent<Floor>();
                HandelFloorCollision(floor);
                break;
            case "Collector":
                Return();
                break;
        }
    }

    private void AddUpwardsImpulse()
    {
        _Rig.AddForce(_impulseForce, ForceMode2D.Impulse);
    }

    private void StartCatReturnTimer()
    {
        _ReturnTimer = _CatReturnTime;
        _WillReturn = true;
    }

    private void HandelFloorCollision(Floor floor)
    {
        switch (_mobType)
        {
            case MobType.CatWithHelmet:
                floor.IncreaseInvasion(2);
                StartCatReturnTimer();
                break;
            case MobType.Cat:
                floor.IncreaseInvasion(1);
                StartCatReturnTimer();
                break;
            case MobType.Dog:
                floor.DogKilled();
                Return();
                break;
        }
    }

    private void ReturnHelmet(Helmet helmet)
    {
        helmet.transform.SetParent(transform);
        helmet.transform.position = _HelmetPosition;
        helmet.gameObject.SetActive(false);
    }
}
