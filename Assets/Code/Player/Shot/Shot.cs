using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public float AliveTime = 0f;

    public float FireForce => _FireForce;
    public float Damage => _ShotDamage;
    
    public void Fire(Vector3 pos)
    {
        transform.position = pos;
        SetActive(true);
        _Rig.AddForce(Vector2.up * FireForce, ForceMode2D.Impulse);
        AliveTime = 3f;
    }

    private Vector3 _ShotSize = new Vector3(0.1f, 0.3f, 1f);
    public void SetShotSize(float shotSize)
    {
        _ShotSize.x = shotSize;
        _Rend.transform.localScale = _ShotSize;
        _Collider.size = _ShotSize;
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

    public void SetShotVelocity(float shotVelocity)
    {
        _FireForce = shotVelocity;
    }

    [SerializeField] SpriteRenderer _Rend;
    private Rigidbody2D _Rig;
    private BoxCollider2D _Collider;
    private float _ShotDamage = 10f, _FireForce;

    void Awake()
    {
        _Rig = GetComponent<Rigidbody2D>();
        _Collider = GetComponent<BoxCollider2D>();
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
