using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiation : MonoBehaviour
{
    public void DoExplode()
    {
        _Effect.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    public GameObject explosion; //prefab
    private GameObject _Effect;
    void Start()
    {
        //StartCoroutine(CauseExplosion());
        _Effect = Instantiate(explosion, new Vector3(0, -3f, 0), Quaternion.identity);
        _Effect.SetActive(false);
        _Effect.transform.SetParent(transform);
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
