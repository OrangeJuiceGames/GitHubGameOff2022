using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : State<GameState>
{
    public override void OnEnter()
    {
        _View.SetActive(true);
    }
    private HelpScreenView _View;
    public HelpScreen(HelpScreenView view, GameState tag) : base(tag)
    {
        _View = view;
    }
}