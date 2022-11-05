using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessor : Updatable
{
    public event Action<float> OnHorizontalMovement;

    private float _Horizontal;
    private readonly string _MoveAxis = "Horizontal";
    public void Update()
    {
        _Horizontal = Input.GetAxis(_MoveAxis);
        OnHorizontalMovement?.Invoke(_Horizontal);
    }
}