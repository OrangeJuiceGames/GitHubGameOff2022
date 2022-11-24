using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helmet : MonoBehaviour, UpgradeMaterial
{
    public event Action<Helmet> OnReturn;

    public void Detatch()
    {
        _Collider.enabled = true;
        _Rig = gameObject.AddComponent<Rigidbody2D>();
        AddRandomForce();
    }

    public void Attach()
    {
        _Collider.enabled = false;
        Destroy(_Rig);
    }

    [SerializeField]
    private float _UpForce = 3f;
    private Rigidbody2D _Rig;
    private BoxCollider2D _Collider;
    private Vector3 _RandomForce = new Vector3(0, 0f);
    private WTMK _Tools = WTMK.Instance;

    private void Awake()
    {
        _Collider = GetComponent<BoxCollider2D>();
        _Collider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Mob collided with " + collision.gameObject.name);
        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                var shot = collision.gameObject.GetComponent<Shot>();
                HandelShotCollision(shot);
                break;
            case "Floor":
                var floor = collision.gameObject.GetComponent<Floor>();
                HandelFloorCollision(floor);
                break;
        }
    }

    private void HandelShotCollision(Shot shot)
    {
        shot.Return();
        OnReturn?.Invoke(this);
    }

    private void HandelFloorCollision(Floor floor)
    {
        floor.UpgradeCollected(this);
        OnReturn?.Invoke(this);
    }

    private void AddRandomForce()
    {
        RollRandomForce();
        _Rig.AddForce(_RandomForce, ForceMode2D.Impulse);
    }

    private void RollRandomForce()
    {
        var roll = _Tools.Rando.NextDouble();
        _RandomForce = new Vector3((float)roll, _UpForce);
    }

}
