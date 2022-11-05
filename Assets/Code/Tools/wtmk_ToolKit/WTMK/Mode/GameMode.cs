using System.Collections;
using System.Collections.Generic;
using System;
public class GameMode : State<string>
{
    public event Action OnModeChange;
    private EventManager _EventManager = EventManager.Instance;

    public GameMode(string tag) : base(tag)
    {
    }

    protected virtual void RegisterGameScreenEvents()
    {
        _EventManager.RegisterEventCallback(GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.RegisterEventCallback(GameScreenEvent.GameSelected.ToString(), OnGameSelected);
        _EventManager.RegisterEventCallback(GameScreenEvent.ModeExit.ToString(), OnModeExit);
    }

    protected virtual void UnregisterGameScreenEvents()
    {
        _EventManager.UnregisterEventCallback(GameScreenEvent.GameSelect.ToString(), OnGameSelect);
        _EventManager.UnregisterEventCallback(GameScreenEvent.GameSelected.ToString(), OnGameSelected);
        _EventManager.UnregisterEventCallback(GameScreenEvent.ModeExit.ToString(), OnModeExit);
    }

    protected virtual void OnGameSelect(string name, object data)
    {
        //override to unregister inputs
    }

    protected virtual void OnGameSelected(string name, object data)
    {
        string nextGame = (string)data;

        if (Tag != nextGame)
        {
            //TO:DO check for valid transtion
            NextState = nextGame;
            _Ready = true;

            OnModeChange?.Invoke();
        }
    }

    protected virtual void OnModeExit(string name, object data)
    {
        string exit = (string)name;

        if(Tag == exit)
        {

        }

    }

}
