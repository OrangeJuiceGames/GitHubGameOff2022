using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public event Action<MobType> OnScore;
    public event Action<UpgradeMaterial> OnUpgradeCollected;

    private List<MobType> _Scoreable = new List<MobType>() { MobType.Cat, MobType.Dog };
    private void OnCollisionEnter2D(Collision2D collision)
    {         
        if(collision.gameObject.name == "Helmet")
        {
            var helmet = collision.gameObject.GetComponent<Helmet>();
            OnUpgradeCollected?.Invoke(helmet);
            helmet.Destroy();
            return;
        }

        var mob = collision.gameObject.GetComponent<Mob>();
        if(mob == null)
        {
            return;
        }

        Collect(mob);
    }

    private void Collect(Mob mob)
    {
        var type = mob.GetMobType();

        if(!_Scoreable.Contains(type))
        {
            return;
        }

        mob.Return();       
        OnScore?.Invoke(type);
    }    
}
