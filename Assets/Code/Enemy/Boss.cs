using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public event Action OnDestroyed;
    [SerializeField]
    private Animator _Effect;

    public void Destroy()
    {
        OnDestroyed?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                var shot = collision.gameObject.GetComponent<Shot>();
                HandelShotCollision(shot);
                break;
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        //deal damage to ship 
        _Effect.transform.position = shot.transform.position;
        _Effect.SetTrigger("Hit");

        //OnDestroyed?.Invoke();
    }
}
