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
    }

    private InputProcessor _Input;
    private Vector2 _MoveVector;

    private void MoveHorizontal(float moveValue)
    {
        _MoveVector.x = moveValue;    
    }

    private void FixedUpdate()
    {
        _Rig.velocity = _MoveVector * _MovePower;
    }
}
