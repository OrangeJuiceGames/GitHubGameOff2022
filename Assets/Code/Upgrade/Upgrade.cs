using System;

public class Upgrade
{
    public event Action<UpgradeResult> OnUpgradeActive;
    private int _Level = 0;
    private int _Exp = 0;
    private int _ExpMax = 10;

    private Floor _Floor;

    public Upgrade(Floor floor)
    {
        _Floor = floor;
        _Floor.OnUpgradeCollected += UpgradeCollected;
    }


    private void UpgradeCollected(UpgradeMaterial upgrade)
    {
        _Exp += 1;

        if (_Exp == _ExpMax)
        {
            _Level++;
            _ExpMax *= _Level;
            _Exp = 0;

            OnUpgradeActive?.Invoke(null);
        }
    }
}
