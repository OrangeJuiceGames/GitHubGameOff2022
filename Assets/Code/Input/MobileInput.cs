using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobileInput : MonoBehaviour
{
    [SerializeField]
    private MobileButton _Left, _Right, _Center, _Screen;
    private InputProcessor _InputProcessor;

    public void Init(InputProcessor ip)
    {
        _InputProcessor = ip;

        _Left.OnUp += OnUp_Left;
        _Left.OnDown += OnDown_Left;

        _Right.OnUp += OnUp_Right;
        _Right.OnDown += OnDown_Right;

        _Center.OnDown += OnDown_Center;
        _Screen.OnDown += OnDown_Screen;
    }

    private void OnUp_Left()
    {
        _InputProcessor.Mobile = false;
        _InputProcessor.Horizontal = 0;
    }

    private void OnDown_Left()
    {
        _InputProcessor.Mobile = true;
        _InputProcessor.Horizontal = -1;
    }

    private void OnUp_Right()
    {
        _InputProcessor.Mobile = false;
        _InputProcessor.Horizontal = 0;
    }

    private void OnDown_Right()
    {
        _InputProcessor.Mobile = true;
        _InputProcessor.Horizontal = 1;
    }

    private void OnDown_Screen()
    {
        _InputProcessor.FireEDown();
    }

    private void OnDown_Center()
    {
        _InputProcessor.FireSpaceDown();
    }
}
