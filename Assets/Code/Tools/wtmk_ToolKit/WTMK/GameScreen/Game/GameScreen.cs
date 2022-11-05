using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : State<GameState>
{
    public override void OnEnter() 
    {
        _View.SetActive(true);
        _Input.OnHorizontalMovement += MoveHorizontal;
    }

    public override void OnExit() 
    {
        _Ready = false;
        _Input.OnHorizontalMovement -= MoveHorizontal;
        _View.SetActive(false);
    }

    public override bool OnUpdate() 
    {
        _Input.Update();
        return _Ready;
    }

    private GameData _GameData = GameData.Instance;
    private GameScreenView _View;

    // Game componets
    private InputProcessor _Input;

    public GameScreen(GameScreenView view, GameState tag) : base(tag)
    {
        _View = view;
        InitGameComponets();
    }

    private void InitGameComponets()
    {
        _Input = new InputProcessor();
    }

    private void MoveHorizontal(float moveValue)
    {
        Debug.Log(moveValue);
    }
}


public enum GameScreenEvent
{
    GameSelect,
    GameSelected,
    GameSelectExit,
    ModeExit
}