using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem 
{
    public static readonly int _CAT_VALUE = 3;
    public static readonly int _DOG_VALUE = 2;

    public ScoreModel Model => _Model;

    private Stage _Stage;
    private ScoreModel _Model;
    public ScoreSystem(Stage stage)
    {
        _Model = new ScoreModel();

        _Stage = stage;
        _Stage.Floor.OnDogKilled += DogKilled;
        _Stage.Floor.OnInvasionIncrease += InvasionIncreased;
        _Stage.Floor.OnCatEscaped += MobScored;
        _Stage.Player.Collector.OnScore += MobScored;
        _Stage.Player.Collector.OnUpgradeCollected += UpgradeCollected;
    }

    private void DogKilled(int value)
    {
        _Model.DogsDead++;
        _Stage.ScoreView.DogsDead.SetText($"Dogs Killed : {_Model.DogsDead}");
    }

    private void InvasionIncreased(int value)
    {
        _Model.InvasionScore++;
        _Stage.ScoreView.Invasion.SetText($"Invasion: {_Model.InvasionScore.ToString("p")}");
    }

    private void MobScored(MobType type)
    {
        //Debug.Log(type);
        if(type == MobType.Dog)
        {
            _Model.Score += _DOG_VALUE;
        }else if(type == MobType.Cat)
        {
            _Model.Score += _CAT_VALUE;
        }

        _Stage.ScoreView.Score.SetText($"{_Model.Score}");
    }

    private void UpgradeCollected(UpgradeMaterial upgrade)
    {
        _Stage.Floor.UpgradeCollected(upgrade);
    }
}
