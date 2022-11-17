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
        _Model = new PlayerModel();
        BuildShotPool();

        _Input = input;
        _UpgradeSystem = upgradeSystem;
        _MoveVector = new Vector2(0, 0);
        _Gun.gameObject.SetActive(false);

        _CombatState = CombatState.Collecting;
    }

    private Pool _ShotPool;
    private List<IPoolable> _Shots;
    private InputProcessor _Input;
    private Upgrade _UpgradeSystem;
    private PlayerModel _Model;
    private Vector2 _MoveVector;
    private Vector3 _180 = new Vector3(0, 180, 0);
    private CombatState _CombatState;

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
            Debug.Log("GunSwapped");
        }
    }
    private void SwapToBasket(float basketValue)
    {
        if (basketValue != 0)
        {
            _CollectorObject.SetActive(true);
            _GunObject.SetActive(false);
            Debug.Log("CollectorSwapped");
        }
    }

    private void TryShoot()
    {
        if(_CombatState == CombatState.Shooting)
        {
            Shoot(1);
        }
    }

    private void Shoot(float shootValue)
    {
        if (shootValue != 0)
        {
            var currentShot = (Shot)_ShotPool.GetPoolable();
            currentShot.Fire(_Gun.ShotSpawnPoint.position);
        }
    }
    
    private void LevelUp(UpgradeResult upgrade)
    {
        Debug.Log("level up");
    }

    private void Update()
    {
        Flip();
    }

    private void FixedUpdate()
    {
        _Rig.velocity = _MoveVector * _MovePower;
    }

    private void Flip()
    {
        if (_MoveVector.x > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (_MoveVector.x < 0)
        {
            transform.eulerAngles = _180;
        }
    }
}


public enum CombatState
{
    Collecting,
    Shooting
}