using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProcessor : Updatable
{
    public event Action<float> OnHorizontalMovement;
    public event Action<float> OnSwapToGun;
    public event Action<float> OnSwapToBasket;
    public event Action<float> OnShoot;

    public event Action OnMouse1Down;
    public event Action OnF1Down;
    public event Action OnEDown;
    public event Action OnLeftShiftDown;
    public event Action OnLeftControlDown;
    public event Action OnLeftAltDown;
    public event Action OnSpaceDown;
    public event Action OnEnterDown;


    private float _Horizontal;
    private float _SwapToGun;
    private float _SwapToBasket;
    private float _Shoot;

    private readonly string _MoveAxis = "Horizontal";
    private readonly string SwapToGun = "SwapToGun";
    private readonly string SwapToBasket = "SwapToBasket";
    private readonly string Shoot = "Shoot";
    public void Update()
    {
        _Horizontal = Input.GetAxis(_MoveAxis);

        if(Input.GetMouseButtonDown(0))
        {
            OnMouse1Down?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            OnF1Down?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            OnEDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnLeftShiftDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            OnLeftControlDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            OnLeftAltDown?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpaceDown?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            OnEnterDown?.Invoke();
        }

        OnHorizontalMovement?.Invoke(_Horizontal);

        /*
        OnSwapToBasket?.Invoke(_SwapToBasket);
        OnSwapToGun?.Invoke(_SwapToGun);
        OnShoot?.Invoke(_Shoot);
        _SwapToGun = Input.GetAxis(SwapToGun);
        _SwapToBasket = Input.GetAxis(SwapToBasket);
        _Shoot = Input.GetAxis(Shoot);
        */
    }
}