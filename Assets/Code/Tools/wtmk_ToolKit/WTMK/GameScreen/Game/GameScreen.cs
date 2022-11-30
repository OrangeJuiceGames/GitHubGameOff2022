using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GameScreen : State<GameState>
{
    public override void OnEnter() 
    {
        if(_Paused)
        {
            _Paused = false;
            return;
        }

        if(_States.CurrentState == GameScreenState.Init)
        {
            _View.SetActive(true);
            _View.Player.Register();
            
            _States.StateChange(GameScreenState.Intro);
        }
    }

    public override void OnExit() 
    {
        _Ready = false;

        if(_Paused)
        {
            return;
        }

        _View.Player.Unregister();
        _View.SetActive(false);
        _States.StateChange(GameScreenState.Init);
    }

    public override bool OnUpdate() 
    {
        if(_States.CurrentState == GameScreenState.Game)
        {
            _Input.Update();
            _WaveSystem.Update();
        }
        
        return _Ready;
    }

    private GameData _GameData = GameData.Instance;
    private GameScreenView _View;

    // Game componets
    private InputProcessor _Input;
    private Upgrade _UpgradeSystem;
    private WaveSystem _WaveSystem;
    private ScoreSystem _ScoreSystem;
    private bool _Paused;
    private StateActionMap<GameScreenState> _States;

    public GameScreen(GameScreenView view, GameState tag) : base(tag)
    {
        _View = view;
        BuildStates();
        InitGameComponets();
        RegisterInput();
    }
    
    private void BuildStates()
    {
        _States = new StateActionMap<GameScreenState>();

        _States.RegisterEnter(GameScreenState.Intro, OnEnter_Intro);
        _States.RegisterEnter(GameScreenState.Game, OnEnter_Game);
        _States.RegisterEnter(GameScreenState.End, OnEnter_End);        
        _States.RegisterEnter(GameScreenState.Paused, OnEnter_Paused);

        _States.RegisterExit(GameScreenState.End, OnExit_End);

        _States.StateChange(GameScreenState.Init);
    }

    private void OnEnter_Intro()
    {
        _View.Story.Intro();
    }

    private void IntroComplete()
    {
        _States.StateChange(GameScreenState.Game);
    }

    private void OnEnter_Game()
    {
        _WaveSystem.Init();
        _ScoreSystem.Init();
        _UpgradeSystem.Init();
    }

    private void OnEnter_End()
    {
        Debug.Log("Game Over");
        _View.GamEnd.SetActive(true);
        _View.Stage.UID.SetText(_ScoreSystem.Model.Score.ToString());
       
        //_View.StartCoroutine(Post("https://rcad-backend.herokuapp.com/user",
        //    "{ user_id:" + _View.ID.Value + ", score:" + _ScoreSystem.Model.Score + "}"));
    }

    private void OnExit_End()
    {
        _View.GamEnd.SetActive(false);
    }

    private void OnEnter_Paused()
    {

    }

    private void RegisterInput()
    {
        _Input.OnEnterDown += TryPause;
    }


    private void InitGameComponets()
    {
        _Input = new InputProcessor();
        _UpgradeSystem = new Upgrade(_View.Stage);
        
        _WaveSystem = new WaveSystem(_View.Stage);
        _WaveSystem.OnGameEnd += EndGameTriggerd;

        _View.Player.Init(_Input, _UpgradeSystem);
        _View.Story.OnStoryComplete += IntroComplete;

        _ScoreSystem = new ScoreSystem(_View.Stage);
        _ScoreSystem.OnGameEnd += EndGameTriggerd;

        _View.Restart.onClick.AddListener(OnClick_Restart);
        _View.Exit.onClick.AddListener(OnClick_Exit);
        _View.GamEnd.SetActive(false);
    }

    private void OnClick_Restart()
    {
        _States.StateChange(GameScreenState.Game);
    }

    private void OnClick_Exit()
    {
        NextState = GameState.Credits;
        _Ready = true;
    }

    private void TryPause()
    {
        _Paused = true;
        NextState = GameState.Help;
        _Ready = true;
    }

    private void EndGameTriggerd()
    {
        _States.StateChange(GameScreenState.End);
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", _View.ID.Value);
        form.AddField("score", _ScoreSystem.Model.Score);


        UnityWebRequest www = UnityWebRequest.Post("https://rcad-backend.herokuapp.com/user", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
    }
}

public enum GameScreenState
{
    Init,
    Intro,
    Game,
    End,
    Paused
}

public enum GameScreenEvent
{
    GameSelect,
    GameSelected,
    GameSelectExit,
    ModeExit
}