using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class StartScreen : State<GameState>
{
    public override void OnEnter() 
    {
        _View.bStart.onClick.AddListener(TransitionToGame);
        _View.SetActive(true);

        
        _View.StartCoroutine(GetRequest("https://rcad-backend.herokuapp.com/user"));
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
    private Dood _Debug = Dood.Instance;
    private Leaderboard _Leaderboard;

    public StartScreen(StartScreenView view, GameState tag) : base(tag)
    {
        _View = view;
        _Leaderboard = new Leaderboard();

        _View.Credits.onClick.AddListener(OnClick_Credits);
    }

    private void OnClick_Credits()
    {
        NextState = GameState.Credits;
        _Ready = true;
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

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Access-Control-Allow-Credentials", "true");
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            webRequest.SetRequestHeader("Access-Control-Allow-Methods", "GET");

            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    _Debug.Error(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    _Debug.Error(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    _Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    var data = webRequest.downloadHandler.text;
                    _View.Leaderboard_Names.SetText(data);
                    break;
            }
        }
    }
}