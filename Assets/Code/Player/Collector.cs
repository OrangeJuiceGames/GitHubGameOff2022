using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with collector!");
        if (collision.gameObject.name == "Cat")
        {
            
        }
        else if (collision.gameObject.name == "CatWithHelmet")
        {

        }
        else if (collision.gameObject.name == "Dog")
        {

        }
    }
}
