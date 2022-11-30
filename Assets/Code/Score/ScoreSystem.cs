using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem 
{
    public event Action OnGameEnd;

    public static readonly int _CAT_VALUE = 3;
    public static readonly int _DOG_VALUE = 2;

    public ScoreModel Model => _Model;

    public void Init()
    {
        _Model = new ScoreModel();
        _Stage.ScoreView.DogsDead.SetText($"Dogs in after life : {_Model.DogsDead}");
        var pre = _Model.InvasionScore * .01f;
        _Stage.ScoreView.Invasion.SetText($"Invasion: {pre.ToString("p")}");

        _Stage.ScoreView.Score.SetText(SpriteTextUtility.ConvertToTextImage($"{_Model.Score}"));
    }

    private Stage _Stage;
    private ScoreModel _Model;
    private WTMK _Tools = WTMK.Instance;
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

    private int _DogKillLimit = 6;
    private void DogKilled(int value)
    {
        _Model.DogsDead++;
        _Stage.ScoreView.DogsDead.SetText($"Dogs in after life : {_Model.DogsDead}");

        if(_Model.DogsDead > _DogKillLimit)
        {
            Debug.LogWarning("game over");
            OnGameEnd?.Invoke();
        }
    }

    private int _MaxInvasion = 100;
    private void InvasionIncreased(int value)
    {
        _Model.InvasionScore += (float)_Tools.Rando.NextDouble();
        var pre =  _Model.InvasionScore * .01f;
        _Stage.ScoreView.Invasion.SetText($"Invasion: {pre.ToString("p")}");

        if(_Model.InvasionScore > _MaxInvasion)
        {
            Debug.LogWarning("game over");
            OnGameEnd?.Invoke();
        }
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

        _Stage.ScoreView.Score.SetText(SpriteTextUtility.ConvertToTextImage($"{_Model.Score}"));
    }

    private void UpgradeCollected(UpgradeMaterial upgrade)
    {
        _Stage.Floor.UpgradeCollected(upgrade);
    }
}
