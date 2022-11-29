using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : State<GameState>
{
    public override void OnEnter()
    {
        _View.SetActive(true);
    }

    public override void OnExit()
    {
        _Ready = false;
        _View.SetActive(false);
    }

    public override bool OnUpdate()
    {
        _Input.Update();
        return _Ready;
    }

    private HelpScreenView _View;
    private InputProcessor _Input;
    public HelpScreen(HelpScreenView view, GameState tag) : base(tag)
    {
        _View = view;
        RegisterInput();
    }

    private void RegisterInput()
    {
        _Input = new InputProcessor();
        _Input.OnEnterDown += ExitPause;
    }

    private void ExitPause()
    {
        NextState = GameState.Game;
        _Ready = true;
    }
        
}