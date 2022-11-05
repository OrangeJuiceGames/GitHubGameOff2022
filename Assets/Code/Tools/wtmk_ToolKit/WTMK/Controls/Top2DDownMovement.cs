using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top2DDownMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _Rigidbody2d;
    [SerializeField]
    private float _Speed;

    protected bool _IsActive = true;
    protected float _Vertical, _Horizontal;
    protected Vector2 _Movement;
    protected EventManager _EventManager = EventManager.Instance;

    private void Update()
    {
        if (!_IsActive)
        {
            return;
        }

        _Vertical = Input.GetAxis("Vertical");
        _Horizontal = Input.GetAxis("Horizontal");
        Debug.Log(_Horizontal);
    }

    void FixedUpdate()
    {
        if(!_IsActive)
        {
            return;
        }

        _Movement.x = _Horizontal;
        _Movement.y = _Vertical;
        _Movement *= Time.fixedDeltaTime * _Speed;
        _Rigidbody2d.MovePosition(_Rigidbody2d.position + _Movement);
    }

    protected virtual void RegisterCallback()
    {
        
    }
}
