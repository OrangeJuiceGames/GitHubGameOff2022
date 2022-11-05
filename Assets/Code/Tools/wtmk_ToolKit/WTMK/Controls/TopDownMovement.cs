using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _Rigidbody;
    [SerializeField]
    private float _Speed;

    protected bool _IsActive;
    protected float _Vertical, _Horizontal;
    protected Vector2 _Movement;
    protected EventManager _EventManager = EventManager.Instance;

    private void Update()
    {
        if (_IsActive)
        {
            return;
        }

        _Vertical = Input.GetAxis("Vertical");
        _Horizontal = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if(_IsActive)
        {
            return;
        }

        _Movement.x = _Horizontal;
        //_Movement.y = _Vertical;
        _Movement *= Time.fixedDeltaTime * _Speed;
        _Rigidbody.MovePosition((Vector2)_Rigidbody.position + _Movement);
    }

    protected virtual void RegisterCallback()
    {
        
    }
}
