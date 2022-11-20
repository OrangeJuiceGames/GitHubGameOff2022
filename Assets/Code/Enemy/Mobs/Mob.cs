using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    [SerializeField]
    private RuntimeAnimatorController _Dog, _Cat;

    private MobType _mobType = MobType.Cat;
    private Vector3 _impulseForce = new Vector3(0, 20f);
    private Rigidbody2D _Rig;
    private Animator _Animator;

    private StateActionMap<MobType> _SkinMob;

    // Start is called before the first frame update
    void Start()
    {
        _Rig = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SkinMob = new StateActionMap<MobType>();
        _SkinMob.RegisterEnter(MobType.Cat, OnEnter_Cat);
        _SkinMob.RegisterEnter(MobType.CatWithHelmet, OnEnter_CatWithHelmet);
        _SkinMob.RegisterEnter(MobType.Dog, OnEnter_Dog);
    }

    private void OnEnter_Cat()
    {
        _Animator.runtimeAnimatorController = _Cat;
        //remove helmet
    }

    private void OnEnter_CatWithHelmet()
    {
        _Animator.runtimeAnimatorController = _Cat;
        //turn on helmet
    }

    private void OnEnter_Dog()
    {
        _Animator.runtimeAnimatorController = _Dog;
    }

    // Update is called once per frame
    void Update()
    {
        
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
                if(_mobType == MobType.CatWithHelmet)
                {
                    return;
                }
                Return();
                break;
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

    private void AddUpwardsImpulse()
    {
        _Rig.AddForce(_impulseForce, ForceMode2D.Impulse);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        if(isActive)
        {
            _SkinMob.StateChange(_mobType);
        }
        
    }

    public void Return()
    {
        _Rig.velocity = Vector2.zero;
        OnReturnRequest?.Invoke(this);
    }
}
