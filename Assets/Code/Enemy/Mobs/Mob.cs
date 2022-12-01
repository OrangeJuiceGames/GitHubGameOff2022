using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public bool HasCollected => _DogCollected;

    public void SetAnimationTrigger(string id)
    {
        _Animator.SetTrigger(id);
    }

    public void SetAnimationBool(string id, bool isActive)
    {
        _Animator.SetBool(id, isActive);
    }

    public void PlayAnimation(string anim)
    {
        _Animator.Play(anim);
    }

    private float _DefaultGravityScale = 0.325f;

    public void Spawn(MobType mobType, Vector3 pos)
    {
        _Rig.mass = 4.75f;
        _Rig.velocity = Vector3.zero;
        _Rig.angularVelocity = 0f;
        _Rig.gravityScale = _DefaultGravityScale;
        transform.position = pos;
        ChangeMobType(mobType);
        SetActive(true);
        
        PlaySpawnAudioForAnimal( mobType );
    }

    public void Return()
    {
        _Collider.isTrigger = false;
        _Rig.velocity = Vector2.zero;
        _Rig.gravityScale = _DefaultGravityScale;
        OnReturnRequest?.Invoke(this);
    }

    public void DogCollected()
    {
        _DogCollected = true;
        _ReturnTimer = _CatReturnTime;
        _Animator.SetBool("Return", true);
        var roll = _Tools.Rando.Next(9);
        if (roll >= 4)
        {
            _ReturnPosition = _Stage.LeftWall.position;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            _ReturnPosition = _Stage.RightWall.position;
            transform.eulerAngles = _180;
        }
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
    
    private Stage _Stage;
    [SerializeField]
    private float _CatReturnTime = 10f;

    private MobType _mobType = MobType.Cat;
    private Vector3 _impulseForce = new Vector3(0, 20f);
    private Vector3 _HelmetPosition, _ReturnPosition;    
    private Rigidbody2D _Rig;
    private Animator _Animator;
    private BoxCollider2D _Collider;

    private StateActionMap<MobType> _SkinMob;
    private float _ReturnTimer = 4f;
    private bool _WillReturn, _DogCollected;
    private Vector3 _180 = new Vector3(0, 180, 0);
    private WTMK _Tools = WTMK.Instance;

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _Collider = GetComponent<BoxCollider2D>();

        _SkinMob = new StateActionMap<MobType>();
        _SkinMob.RegisterEnter(MobType.Cat, OnEnter_Cat);
        _SkinMob.RegisterEnter(MobType.CatWithHelmet, OnEnter_CatWithHelmet);
        _SkinMob.RegisterEnter(MobType.Dog, OnEnter_Dog);

        _Helmet.OnReturn += ReturnHelmet;

        _HelmetPosition = _Helmet.transform.position;
    }

    private void Start()
    {
        _Stage = FindObjectOfType<Stage>();
    }

    private void Update()
    {
        DoReturn();
    }

    private void DoReturn()
    {
        if (_WillReturn)
        {
            _ReturnTimer -= Time.deltaTime;
            var pos = MoveToReturnPosition();
            var target = new Vector3(pos.x, pos.y, 0f);

            if (_mobType == MobType.Dog)
            {
                target.x = transform.position.x;
                transform.position = target;
            }
            else
            {
                target.y = transform.position.y;
                transform.position = target;
            }

            if (_ReturnTimer <= 0)
            {
                _WillReturn = false;
                _ReturnTimer = _CatReturnTime;
                Return();
            }
        }else if(_DogCollected)
        {
            _ReturnTimer -= Time.deltaTime;
            var pos = MoveToReturnPosition();
            var target = new Vector3(pos.x, pos.y, 0f);

            target.y = transform.position.y;
            transform.position = target;

            if (_ReturnTimer <= 0)
            {
                _DogCollected = false;
                _ReturnTimer = _CatReturnTime;
                Return();
            }
        }
    }

    // heheheh, gross
    private void OnEnter_Cat()
    {
        _mobType = _SkinMob.CurrentState;
        _Animator.runtimeAnimatorController = _Cat;
        SetAnimationBool("NoHelmet", true);
        transform.DetachChildren();
        _Helmet.Detatch();
    }

    private void OnEnter_CatWithHelmet()
    {
        _mobType = _SkinMob.CurrentState;
        _Animator.runtimeAnimatorController = _Cat;
        _Helmet.gameObject.SetActive(true);
    }

    private void OnEnter_Dog()
    {
        _mobType = _SkinMob.CurrentState;
        _Animator.runtimeAnimatorController = _Dog;
        _Helmet.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Mob collided with " + collision.gameObject.name);
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                var shot = collision.gameObject.GetComponent<Shot>();
                HandelShotCollision(shot);
                break;
            case "Floor":
                if(!_DogCollected)
                {
                    var floor = collision.gameObject.GetComponent<Floor>();
                    HandelFloorCollision(floor);
                }
                break;
            case "Collector":
                Return();
                break;
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        shot.Return();

        AddUpwardsImpulse();

        if(_mobType == MobType.CatWithHelmet)
        {
            _SkinMob.StateChange(MobType.Cat);
        }
    }

    private void AddUpwardsImpulse()
    {
        _Rig.AddForce(_impulseForce, ForceMode2D.Impulse);
    }

    private void StartReturnTimer()
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
                SetReturnPosition();
                StartReturnTimer();
                _Helmet.gameObject.SetActive(false);
                SetAnimationBool("Walking", true);
                AudioManager.Instance.PlayAudioByEnumType( AudioType.CatLandWithHelmet );
                break;
            case MobType.Cat:
                SetReturnPosition();
                floor.CatEscaped();
                StartReturnTimer();
                SetAnimationBool("Walking", true);
                AudioManager.Instance.PlayAudioByEnumType( AudioType.CatLandWithoutHelmet );
                break;
            case MobType.Dog:
                _Collider.isTrigger = true;
                floor.DogKilled(transform.position);
                SetReturnPosition();
                SetAnimationTrigger("Dead");
                StartReturnTimer();
                AudioManager.Instance.PlayAudioByEnumType( AudioType.DogFailureCatch );
                break;
        }
    }

    private void SetReturnPosition()
    {
        if(_mobType == MobType.Dog)
        {
            _Rig.gravityScale = -_DefaultGravityScale;
            _ReturnPosition = _Stage.Roof.position;
            return;
        }

        var roll = _Tools.Rando.Next(9);
        if (roll >= 4)
        {
            _ReturnPosition = _Stage.LeftWall.position;
            transform.eulerAngles = Vector3.zero;
        }
        else
        {
            _ReturnPosition = _Stage.RightWall.position;
            transform.eulerAngles = _180;
        }
    }

    private Vector3 MoveToReturnPosition()
    {
        return Vector3.MoveTowards(transform.position, _ReturnPosition, 3f * Time.deltaTime);
    }

    private void ReturnHelmet(Helmet helmet)
    {
        helmet.Attach();
        helmet.transform.SetParent(transform);
        helmet.transform.position = _HelmetPosition;
        helmet.gameObject.SetActive(false);
    }
    
    private void PlaySpawnAudioForAnimal( MobType mobThatSpawned )
    {
        AudioManager.Instance.PlayAudioByEnumType( AudioType.ShipThrowAnimal );
        
        AudioManager.Instance.PlayAudioByEnumType( mobThatSpawned == MobType.Dog
            ? AudioType.DogSpawn
            : AudioType.CatSpawn );
    }
}
