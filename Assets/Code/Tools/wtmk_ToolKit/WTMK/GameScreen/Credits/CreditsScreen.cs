using System.Collections;
using System.Collections.Generic;

public class CreditsScreen : State<GameState>
{
    public override void OnEnter() 
    {
        _View.SetActive(true);
    }

    public override void OnExit()
    {
        _View.SetActive(false);
        _Ready = false;
    }

    private CreditsScreenView _View;
    private GameScreenTags _ScreenTags = new GameScreenTags();

    public CreditsScreen(CreditsScreenView view, GameState tag) : base(tag)
    {
        _View = (CreditsScreenView)view;
        _View.Exit.onClick.AddListener(OnClick_Exit);
        _View.UI.SetActive(false);
    }

    private void OnClick_Exit()
    {
        NextState = GameState.Start;
        _Ready = true;
    }
}