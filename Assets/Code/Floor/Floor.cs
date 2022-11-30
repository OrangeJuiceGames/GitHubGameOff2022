using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public event Action<MobType> OnCatEscaped;
    public event Action<int> OnInvasionIncrease;
    public event Action<int> OnDogKilled;
    public event Action<UpgradeMaterial> OnUpgradeCollected;

    [SerializeField]
    private Animator _Effect;

    public void DogKilled(Vector3 pos)
    {
        _Effect.transform.position = pos;
        _Effect.SetTrigger("DogHit");
        OnDogKilled?.Invoke(1);
    }    

    public void CatEscaped()
    {
        OnCatEscaped?.Invoke(MobType.Cat);
    }

    public void IncreaseInvasion(int value)
    {
        OnInvasionIncrease?.Invoke(value);
    }

    public void UpgradeCollected(UpgradeMaterial material)
    {
        OnUpgradeCollected?.Invoke(material);
    }

}
