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
        _SwapToGun = Input.GetAxis(SwapToGun);
        _SwapToBasket = Input.GetAxis(SwapToBasket);
        _Shoot = Input.GetAxis(Shoot);

        OnHorizontalMovement?.Invoke(_Horizontal);
        OnSwapToBasket?.Invoke(_SwapToBasket);
        OnSwapToGun?.Invoke(_SwapToGun);
        OnShoot?.Invoke(_Shoot);


    }
}