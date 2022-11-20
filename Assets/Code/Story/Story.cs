using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    public event Action OnStoryComplete;    

    public void Intro()
    {
        _IntroTimer.Start(IntroTime);

    }

    private Timer _IntroTimer = new Timer();
    [SerializeField] float IntroTime = 1000f;

    private void Start()
    {
        _IntroTimer.OnTimerComplete += OnStoryComplete;
    }

    public void Update()
    {
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        _IntroTimer.Update();
    }
}
