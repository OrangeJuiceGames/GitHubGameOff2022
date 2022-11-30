using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour
{
    public event Action OnExplosionComplete;
    public GameObject explosion; //prefab

    public void DoExplode()
    {
        StartCoroutine(CauseExplosion());
    }

    IEnumerator CauseExplosion()
    {
        yield return new WaitForSeconds(.3f);
        Instantiate(explosion, transform);
        yield return new WaitForSeconds(.4f);
        Instantiate(explosion, transform);
        yield return new WaitForSeconds(.5f);
        Instantiate(explosion, transform);
        yield return new WaitForSeconds(1f);
        OnExplosionComplete?.Invoke();
    }
}
