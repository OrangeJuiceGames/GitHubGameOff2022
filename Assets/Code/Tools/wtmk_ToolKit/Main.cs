using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// This class should be duplicated since everything in it will be customized per game
public class Main : MonoBehaviour
{
    // Game screen views
    [SerializeField]
    private StartScreenView _StartScreenView;
    [SerializeField]
    private GameScreenView _GameScreenView;
    [SerializeField]
    private HelpScreenView _HelpScreenView;
    [SerializeField]
    private CreditsScreenView _CreditScreenView;
    [SerializeField]
    private UserID _UserId;

    // Game screens
    private StartScreen _StartScreen;
    private GameScreen _GameScreen;
    private HelpScreen _HelpScreen;
    private CreditsScreen _CreditsScreen;

    private StateMachine<GameState> _GameScreenDirector;

    private Dood _Dood = Dood.Instance;
    private GameScreenTags _ScreenTags;

    private ApplicationState _CurrentState;

    private void Awake()
    {
        _CurrentState = ApplicationState.Init;

        _ScreenTags = new GameScreenTags();
        InitGameScreens(BuildGameScreens());
    }

    private void Start()
    {
        Dood.IsLogging = true;
        _GameScreenDirector.StateChange(GameState.Start);
        _GameScreenDirector.IsActive = true;

        _CurrentState = ApplicationState.Running;

        if(!PlayerPrefs.HasKey("userID"))
        {
            PlayerPrefs.SetString("userID", "");
            PlayerPrefs.Save();
        }

        string userID = PlayerPrefs.GetString("userID");

        if(userID == "")
        {
            Debug.LogError("create id");
            var date = DateTime.Now;
            userID = System.Environment.TickCount.ToString();
            PlayerPrefs.SetString("userID", userID);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    { 
        if(_CurrentState == ApplicationState.Init)
        {
            return;
        }

        _GameScreenDirector.OnUpdate();
    }

    private IState<GameState>[] BuildGameScreens()
    {
        _StartScreen = new StartScreen(_StartScreenView, GameState.Start);
        _GameScreen = new GameScreen(_GameScreenView, GameState.Game);
        _HelpScreen = new HelpScreen(_HelpScreenView, GameState.Help);
        _CreditsScreen = new CreditsScreen(_CreditScreenView, GameState.Credits);

        _StartScreen.SetValidTransitions(new GameState[] { _GameScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag });
        _GameScreen.SetValidTransitions(new GameState[] { _StartScreen.Tag, _HelpScreen.Tag, _CreditsScreen.Tag });
        _HelpScreen.SetValidTransitions(new GameState[] { _StartScreen.Tag, _GameScreen.Tag });
        _CreditsScreen.SetValidTransitions(new GameState[] { _StartScreen.Tag, _GameScreen.Tag });

        IState<GameState>[] gameStates = new IState<GameState>[] { _StartScreen, _GameScreen, _HelpScreen, _CreditsScreen };

        return gameStates;
    }


    private void InitGameScreens(IState<GameState>[] gameStates)
    {
        if (gameStates == null)
        {
            Debug.LogError("Error: Can't init game screens.");
        }

        _GameScreenDirector = new StateMachine<GameState>(gameStates);
    }

}

public enum ApplicationState
{
    Init,
    Running,
    Paused,
}

public enum GameState
{
    Start,
    Game,
    Help,
    Credits
}
