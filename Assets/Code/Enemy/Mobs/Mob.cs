using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    private Vector3 _impulseForce = new Vector3(0, 20f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Shot(Clone)")
        {
            AddUpwardsImpulse();
        }
        else if (collision.gameObject.name == "Floor")
        {
            Destroy(gameObject);
        }
    }

    private void AddUpwardsImpulse()
    {
        GetComponent<Rigidbody2D>().AddForce(_impulseForce, ForceMode2D.Impulse);
    }






}
