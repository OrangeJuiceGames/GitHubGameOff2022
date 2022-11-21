using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : MonoBehaviour, UpgradeMaterial
{
    public event Action<Helmet> OnReturn;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Mob collided with " + collision.gameObject.name);
        switch (collision.gameObject.name)
        {
            case "Floor":
                var floor = collision.gameObject.GetComponent<Floor>();
                HandelFloorCollision(floor);
                break;
        }
    }

    private void HandelFloorCollision(Floor floor)
    {
        floor.UpgradeCollected(this);
        OnReturn?.Invoke(this);
    }
}
