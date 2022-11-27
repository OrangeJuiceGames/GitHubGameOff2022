using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject explosion;
    void Start()
    {
        StartCoroutine(CauseExplosion());
    }
    IEnumerator CauseExplosion()
    {
        Debug.Log("ExplosionMade");
        yield return new WaitForSeconds(5f);
        Instantiate(explosion, new Vector3(0, -3f, 0), Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
