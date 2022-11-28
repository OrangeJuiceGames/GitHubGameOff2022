using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    [SerializeField]
    private SpriteRenderer _Rend;
    [SerializeField]
    private Rigidbody2D _Rig;
    [SerializeField]
    private Animator _Animator;
    [SerializeField]
    private float _MovePower = 1.3f;
    [SerializeField]
    private Collector _Collector;
    [SerializeField]
    private GameObject _CollectorObject;
    [SerializeField]
    private Gun _Gun;
    [SerializeField]
    private GameObject _GunObject;
    [SerializeField]
    private Shot _ShotPrefab;
    [SerializeField]
    private int _ShotPoolSize;

    public PlayerModel Model => _Model;
    public Collector Collector => _Collector;
    
    public void Register()
    {
        _Input.OnHorizontalMovement += MoveHorizontal;

        _Input.OnF1Down += TrySwap;
        _Input.OnEDown += TrySwap;
        _Input.OnLeftShiftDown += TrySwap;
        _Input.OnLeftControlDown += TrySwap;
        _Input.OnLeftAltDown += TrySwap;

        _Input.OnMouse1Down += TryShoot;
        _Input.OnSpaceDown += TryShoot;

        _UpgradeSystem.OnUpgradeActive += LevelUp;

        _Collector.OnScore += Catch;
        _Collector.OnUpgradeCollected += Catch;
    }

    public void Unregister()
    {
        _Input.OnHorizontalMovement -= MoveHorizontal;

        _Input.OnF1Down -= TrySwap;
        _Input.OnEDown -= TrySwap;
        _Input.OnLeftShiftDown -= TrySwap;
        _Input.OnLeftControlDown -= TrySwap;
        _Input.OnLeftAltDown -= TrySwap;

        _Input.OnMouse1Down -= TryShoot;
        _Input.OnSpaceDown -= TryShoot;

        _UpgradeSystem.OnUpgradeActive -= LevelUp;
    }

    public void Init(InputProcessor input, Upgrade upgradeSystem)
    {
        _Input = input;
        _UpgradeSystem = upgradeSystem;
        _MoveVector = new Vector2(0, 0);
        _Gun.gameObject.SetActive(false);

        _CombatState = CombatState.Collecting;

        SetDefaultStats();
        BuildShotPool();
    }

    private Pool _ShotPool;
    private List<IPoolable> _Shots;
    private InputProcessor _Input;
    private Upgrade _UpgradeSystem;
    private PlayerModel _Model;
    private Vector2 _MoveVector;
    private Vector3 _180 = new Vector3(0, 180, 0);
    private CombatState _CombatState;
    private bool _CanShoot = true;
    private bool _facingRight = true;
    private float _RateOfFire;
    

    private void BuildShotPool()
    {
        _Shots = new List<IPoolable>();
        int count = _ShotPoolSize;
        while (count > 0)
        {
            count--;
            var clone = Instantiate(_ShotPrefab);

            var shot = clone.GetComponent<Shot>();
            _Shots.Add(shot);
            clone.transform.position = _Gun.ShotSpawnPoint.position;
        }

        _ShotPool = new Pool(_Shots.ToArray());
    }
    private void SetDefaultStats()
    {
        _Model = _UpgradeSystem.PlayerModel;
    }

    private void MoveHorizontal(float moveValue)
    {
        _MoveVector.x = moveValue;
    }
    private void TrySwap()
    {
        if(_CombatState == CombatState.Collecting)
        {
            _CombatState = CombatState.Shooting;
            SwapToGun(1);
        }else if(_CombatState == CombatState.Shooting)
        {
            _CombatState = CombatState.Collecting;
            SwapToBasket(1);
        }
    }

    private void SwapToGun(float gunValue) 
    {
        if (gunValue != 0)
        {
            _GunObject.SetActive(true);
            _CollectorObject.SetActive(false);
            _Animator.SetBool("HasGun", true);
            AudioManager.Instance.PlayAudioByEnumType(AudioType.characterChangeWeapon);
        }
    }
    private void SwapToBasket(float basketValue)
    {
        if (basketValue != 0)
        {
            _CollectorObject.SetActive(true);
            _GunObject.SetActive(false);
            _Animator.SetBool("HasGun", false);
            AudioManager.Instance.PlayAudioByEnumType(AudioType.characterChangeWeapon);
        }
    }

    private void TryShoot()
    {
        if(_CombatState == CombatState.Shooting) // && _CanShoot)
        {
            Shoot(1);
        }
    }

    private void Shoot(float shootValue)
    {
        if (shootValue != 0)
        {
            _CanShoot = false;
            _RateOfFire = _Model.RateOfFire;
            var currentShot = (Shot)_ShotPool.GetPoolable();
            currentShot.SetShotDamage(_Model.Damage);
            currentShot.Fire(_Gun.ShotSpawnPoint.position);
            _Animator.SetTrigger("Shoot");
            AudioManager.Instance.PlayAudioByEnumType(AudioType.characterFireWeapon);
        }
    }

    private void Catch(MobType type)
    {
        _Animator.SetTrigger("Catch");
    }

    private void Catch(UpgradeMaterial type)
    {
        _Animator.SetTrigger("Catch");
    }

    private void LevelUp(UpgradeResult upgrade)
    {
        Debug.Log("level up");
    }

    private void Update()
    {
        Flip();
        HandleFireRate();
    }

    private void FixedUpdate()
    {
        _Rig.velocity = _MoveVector * _MovePower * Time.deltaTime;
    }

    private void HandleFireRate()
    {
        if(!_CanShoot)
        {
            _RateOfFire -= Time.deltaTime;

            if(_RateOfFire <= 0)
            {
                _CanShoot = true;
            }
        }
    }

    private void Flip()
    {
        if (_MoveVector.x > 0)
        {
            transform.eulerAngles = Vector3.zero;

            // Checks for change in facing direction.
            if ( _facingRight )
                return;

            AudioManager.Instance.PlayAudioByEnumType(AudioType.characterChangeDirection);
            _facingRight = !_facingRight;
        }
        else if (_MoveVector.x < 0)
        {
            transform.eulerAngles = _180;
            
            if ( !_facingRight )
                return;

            AudioManager.Instance.PlayAudioByEnumType(AudioType.characterChangeDirection);
            _facingRight = !_facingRight;
        }
    }
}


public enum CombatState
{
    Collecting,
    Shooting
}