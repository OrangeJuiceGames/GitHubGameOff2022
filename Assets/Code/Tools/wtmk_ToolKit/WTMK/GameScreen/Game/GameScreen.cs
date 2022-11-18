using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : State<GameState>
{
    public override void OnEnter() 
    {
        _View.SetActive(true);
        _View.Player.Register();
    }

    public override void OnExit() 
    {
        _Ready = false;
        _View.Player.Unregister();
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
    private Upgrade _UpgradeSystem;

    public GameScreen(GameScreenView view, GameState tag) : base(tag)
    {
        _View = view;
        InitGameComponets();
    }

    private void InitGameComponets()
    {
        _Input = new InputProcessor();
        _UpgradeSystem = new Upgrade(_View.Floor);
        _View.Player.Init(_Input, _UpgradeSystem);
    }
}


public enum GameScreenEvent
{
    GameSelect,
    GameSelected,
    GameSelectExit,
    ModeExit
}