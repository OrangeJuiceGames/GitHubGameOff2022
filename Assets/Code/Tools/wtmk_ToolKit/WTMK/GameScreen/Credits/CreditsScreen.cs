using System.Collections;
using System.Collections.Generic;

public class CreditsScreen : State<GameState>
{
    private CreditsScreenView _View;
    private GameScreenTags _ScreenTags = new GameScreenTags();

    public CreditsScreen(CreditsScreenView view, GameState tag) : base(tag)
    {
        _View = (CreditsScreenView)view;
    }
}