using System;
using System.Collections;
using System.Collections.Generic;

public class Upgrade
{
    public event Action<UpgradeResult> OnUpgradeActive;
    public PlayerModel PlayerModel => _PlayerData;

    public void Init()
    {
        _Level = 0;
        _Exp = 0;
        _ExpMax = 6;
        _LevelUpFactor = 0.3f;
        _Stage.ExpBar.fillAmount = 0f;
        _Stage.Level.SetText($"Level: {_Level}");

        _PlayerData = new PlayerModel();
    }

    private int _Level = 0;
    private float _Exp = 0;
    private float _ExpMax = 6;
    private float _LevelUpFactor = 0.3f;
    private List<float> _UpgradePotency = new List<float>() { 0.01f, 0.02f, 0.03f };

    private Stage _Stage;
    private PlayerModel _PlayerData;
    private Dictionary<int, Action> _UpgradeOptions;
    private WTMK _Tools = WTMK.Instance;
    private Dood _Debug = Dood.Instance;
    private Dictionary<int, string> _UpgradeLabels;

    public Upgrade(Stage stage)
    {
        _PlayerData = new PlayerModel();
        _PlayerData.RateOfFire = 3f;

        _Stage = stage;
        _Stage.Floor.OnUpgradeCollected += UpgradeCollected;
        BuildOptions();
    }


    private void BuildOptions()
    {
        _UpgradeOptions = new Dictionary<int, Action>();
        _UpgradeLabels = new Dictionary<int, string>();

        _UpgradeOptions.Add(0, UpgradeShotSizeX);
        _UpgradeOptions.Add(10, UpgradeShotSizeX);

        _UpgradeOptions.Add(1, UpgradeShotVelocity);
        _UpgradeOptions.Add(2, UpgradeShotVelocity);
        _UpgradeOptions.Add(4, UpgradeShotVelocity);
        
        _UpgradeOptions.Add(3, UpgradeShotDamage);
        _UpgradeOptions.Add(7, UpgradeShotDamage);
        _UpgradeOptions.Add(9, UpgradeShotDamage);

        _UpgradeOptions.Add(5, UpgradeMoveSpeed);
        _UpgradeOptions.Add(8, UpgradeMoveSpeed);
        _UpgradeOptions.Add(6, UpgradeMoveSpeed);

        _UpgradeLabels.Add(0,  "Shot Size ++");
        _UpgradeLabels.Add(10, "Shot Size ++");

        _UpgradeLabels.Add(1, "Shot Speed ++");
        _UpgradeLabels.Add(2, "Shot Speed ++");
        _UpgradeLabels.Add(4, "Shot Speed ++");

        _UpgradeLabels.Add(3, "Damage ++");
        _UpgradeLabels.Add(7, "Damage ++");
        _UpgradeLabels.Add(9, "Damage ++");

        _UpgradeLabels.Add(5, "Move Speed ++");
        _UpgradeLabels.Add(8, "Move Speed ++");
        _UpgradeLabels.Add(6, "Move Speed ++");
    }


    private void UpgradeCollected(UpgradeMaterial upgrade)
    {
        _Exp += 1;
        var exp = _Exp / _ExpMax * 100;
        _Stage.ExpBar.fillAmount = exp * .01f;

        _Stage.ScrapomaticEffect.SetTrigger("Red");
        _Stage.Level.SetText($"Level: {_Level}");

        if (_Exp >= _ExpMax)
        {
            _Level++;
            _ExpMax += (_Level * _LevelUpFactor);
            _Exp = 0;
            _Stage.ExpBar.fillAmount = 0f;
            
            var roll = _Tools.Rando.Next(_UpgradeOptions.Count);

            if (_UpgradeLabels.ContainsKey(roll))
            {
                _Stage.Level.SetText($"{_UpgradeLabels[roll]}");
            }

            _Stage.ScrapomaticEffect.SetTrigger("Green");

            if (_UpgradeOptions.ContainsKey(roll))
            {
                _UpgradeOptions[roll]();
                OnUpgradeActive?.Invoke(null);
            }
            else
            {
                //Something is not right with upgrade options crash game
                //we should never get here if things are not well
                throw new Exception();
            }
            
            AudioManager.Instance.PlayAudioByEnumType( AudioType.CharacterUpgrade );
        }

        
    }

    private void UpgradeShotSizeX()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.ShotSizeX += _UpgradePotency[roll];
    }

    private void UpgradeShotSizeY()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.ShotSizeY += _UpgradePotency[roll];
    }

    private void UpgradeShotVelocity()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.ShotVelocity += _UpgradePotency[roll];
    }

    private void UpgradeShotDamage()
    {
        var roll = _Tools.Rando.Next(10, 25);

        _PlayerData.Damage += roll;//_UpgradePotency[roll];
    }

    private void UpgradeFireRate()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.RateOfFire -= _UpgradePotency[roll];
    }

    private void AddPenetration()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.Penetration = true;
    }

    private void AddMultiShot()
    {
        _PlayerData.ShotCount++;
    }

    private void UnlockDash()
    {
        _PlayerData.Dash = true;
    }

    private void UpgradeMoveSpeed()
    {
        var roll = _Tools.Rando.Next(10, 25);

        _PlayerData.MoveSpeed += roll;//_UpgradePotency[roll];
    }

    private void LowerDashCoolDown()
    {
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.DashCoolDown -= _UpgradePotency[roll];
    }

    private void ActivateShield()
    {
        _PlayerData.HasShield = true;
    }
}
