using System;
using System.Collections;
using System.Collections.Generic;

public class StartScreen : State<GameState>
{
    public override void OnEnter() 
    {
        _View.bStart.onClick.AddListener(TransitionToGame);
        _View.SetActive(true);
    }
    
    public override void OnExit() 
    {
        _Ready = false;

        _View.SetActive(false);
        _View.bStart.onClick.RemoveAllListeners();    
    }

    public override bool OnUpdate() 
    {
        return _Ready; 
    }

    private GameData _GameData = GameData.Instance;

    private StartScreenView _View;    
    private Dood _Dood = Dood.Instance;

    public StartScreen(StartScreenView view, GameState tag) : base(tag)
    {
        _View = view;
    }

    private void TransitionToGame()
    {
        NextState = GameState.Game;
        _Ready = true;
    }

    private void TransitionToHelp()
    {
        NextState = GameState.Help;
    }
}