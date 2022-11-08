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
    private Gun _Gun;

    public void Register()
    {
        _Input.OnHorizontalMovement += MoveHorizontal;
    }

    public void Unregister()
    {
        _Input.OnHorizontalMovement -= MoveHorizontal;
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
