using System.Collections;
using System.Collections.Generic;

public class GameScreenDirector : IStateDirector
{
    public bool IsActive { get; set; }

    public void OnUpdate()
    {
        if(!IsActive)
        {
            return;
        }

        if (_StateMap[_CurrentState].OnUpdate())
        {
            _PreviousState = _CurrentState;
            _CurrentState = _StateMap[_CurrentState].NextState;

            _StateMap[_PreviousState].OnExit();
            _StateMap[_CurrentState].OnEnter();
        }
    }

    public void SetActive(string screen, bool isActive)
    {
        _StateMap[screen].View.SetActive(isActive);
    }

    private Dood _Debug = Dood.Instance;
    private GameScreenTags ValidScreen = new GameScreenTags();
    private Dictionary<string, IState<string>> _StateMap = new Dictionary<string, IState<string>>();

    private IState<string>[] _States;
    private string _CurrentState;
    private string _PreviousState;

    public GameScreenDirector(IState<string>[] states)
    {
        _States = states;

        HideAllScreens();

        for (int i = 0; i < _States.Length; i++)
        {
            _StateMap.Add(_States[i].Tag, _States[i]);
        }

        _CurrentState = ValidScreen.Start;
        _StateMap[_CurrentState].OnEnter();
    }

    private void HideAllScreens()
    {
        for (int i = 0; i < _States.Length; i++)
        {
            if(_States[i].View == null)
            {
                _Debug.Log($"{_States[i].Tag} View is null");
                return;
            }

            _States[i].View.SetActive(false);
        }
    }
}
