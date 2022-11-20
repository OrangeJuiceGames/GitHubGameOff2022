using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    public event Action<int> OnScore;
    [SerializeField]
    private int _Cat, _Dog;

    private void OnCollisionEnter2D(Collision2D collision)
    {         
        var mob = collision.gameObject.GetComponent<Mob>();
        if(mob == null)
        {
            return;
        }

        Collect(mob);
    }

    private void Collect(Mob mob)
    {
        mob.Return();

        if (mob.GetMobType() == MobType.Cat)
        {
            Debug.Log("Socred Cat!");
            OnScore?.Invoke(_Cat);
        }else if(mob.GetMobType() == MobType.Dog)
        {
            Debug.Log("Socred Dog!");
            OnScore?.Invoke(_Dog);
        }
    }    
}
