using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem 
{
    private Stage _Stage;
    public ScoreSystem(Stage stage)
    {
        _Stage = stage;
        _Stage.Floor.OnDogKilled += DogKilled;
        _Stage.Floor.OnInvasionIncrease += InvasionIncreased;
    }

    private void DogKilled(int value)
    {
        Debug.Log(value);
    }

    private void InvasionIncreased(int value)
    {
        Debug.Log(value);
    }

    private void MobSaved(MobType type)
    {
        Debug.Log(type);
    }
}
