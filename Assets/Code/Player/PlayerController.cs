using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    [SerializeField]
    private SpriteRenderer _Model;
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

    public void Register()
    {
        _Input.OnHorizontalMovement += MoveHorizontal;
        _Input.OnSwapToBasket += SwapToBasket;
        _Input.OnSwapToGun += SwapToGun;
        _Input.OnShoot += Shoot;
    }

    public void Unregister()
    {
        _Input.OnHorizontalMovement -= MoveHorizontal;
        _Input.OnSwapToBasket -= SwapToBasket;
        _Input.OnSwapToGun -= SwapToGun;
        _Input.OnShoot -= Shoot;
    }

    public void Init(InputProcessor input)
    {
        _Input = input;
        _MoveVector = new Vector2(0, 0);
        _Gun.gameObject.SetActive(false);
    }
    private InputProcessor _Input;
    private Vector2 _MoveVector;
    private Vector3 _180 = new Vector3(0, 180, 0);

    private void MoveHorizontal(float moveValue)
    {
        _MoveVector.x = moveValue;    
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
    private void Shoot(float shootValue)
    {
        if (shootValue != 0)
        {
            Debug.Log("Gun has been fired");
        }
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
