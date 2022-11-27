using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public float AliveTime = 0f;
    public float FireForce = 10f;
    public float Damage => _ShotDamage;

    public void Fire(Vector3 pos)
    {
        transform.position = pos;
        SetActive(true);
        _Rig.AddForce(Vector2.up * FireForce, ForceMode2D.Impulse);
        AliveTime = 3f;
    }

    public void Return()
    {
        OnReturnRequest?.Invoke(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetShotDamage(float shotDamage)
    {
        _ShotDamage = shotDamage;
    }

    private Rigidbody2D _Rig;
    private float _ShotDamage = 10f;

    void Awake()
    {
        _Rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(AliveTime > 0)
        {
            AliveTime -= Time.deltaTime;
        }

        if(AliveTime <= 0)
        {
            SetActive(false);
            Return();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.name)
        {
            case "Cat":
                Return();
                break;
            case "Dog":
                Return();
                break;
            case "EnemyShip(Clone)":
                Return();
                break;
        }
    }
}
