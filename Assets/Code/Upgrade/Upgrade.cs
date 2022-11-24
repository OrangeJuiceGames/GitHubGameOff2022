using System;
using System.Collections;
using System.Collections.Generic;

public class Upgrade
{
    public event Action<UpgradeResult> OnUpgradeActive;
    public PlayerModel PlayerModel => _PlayerData;

    private int _Level = 0;
    private float _Exp = 0;
    private float _ExpMax = 6;
    private float _LevelUpFactor = 0.3f;
    private List<float> _UpgradePotency = new List<float>() { 0.01f, 0.02f, 0.03f };

    private Stage _Stage;
    private PlayerModel _PlayerData;
    private Dictionary<int, Action> _UpgradeOptions;
    private WTMK _Tools = WTMK.Instance;

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

        _UpgradeOptions.Add(0, UpgradeShotSizeX);
        _UpgradeOptions.Add(1, UpgradeShotSizeY);
        _UpgradeOptions.Add(2, UpgradeShotVelocity);
        _UpgradeOptions.Add(3, UpgradeShotDamage);
        _UpgradeOptions.Add(4, UpgradeFireRate);
        _UpgradeOptions.Add(5, AddPenetration);
        _UpgradeOptions.Add(6, AddMultiShot);
        _UpgradeOptions.Add(7, UnlockDash);
        _UpgradeOptions.Add(8, UpgradeMoveSpeed);
        _UpgradeOptions.Add(9, LowerDashCoolDown);
        _UpgradeOptions.Add(10, ActivateShield);
    }


    private void UpgradeCollected(UpgradeMaterial upgrade)
    {
        _Exp += 1;
        if (_Exp == _ExpMax)
        {
            _Level++;
            _ExpMax *= _ExpMax + (_Level * _LevelUpFactor);
            _Exp = 0;

            var roll = _Tools.Rando.Next(_UpgradeOptions.Count);
            if(_UpgradeOptions.ContainsKey(roll))
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
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.Damage += _UpgradePotency[roll];
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
        var roll = _Tools.Rando.Next(_UpgradePotency.Count);

        _PlayerData.MoveSpeed += _UpgradePotency[roll];
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
