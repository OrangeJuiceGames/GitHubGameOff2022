using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    private MobType _mobType = MobType.Cat;
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
        Debug.Log("Mob collided with " + collision.gameObject.name);

        switch (collision.gameObject.name)
        {
            case "Shot(Clone)":
                AddUpwardsImpulse();
                break;
            case "Floor":
                if (_mobType == MobType.Cat)
                {
                    Return();
                }
                else if (_mobType == MobType.Dog)
                {
                    Return();
                }
                break;
            case "collector!":
                Return();
                break;
        }
    }

    public MobType GetMobType()
    {
        return _mobType;
    }

    public void ChangeMobType(MobType newMobType)
    {
        _mobType = newMobType;
        transform.name = newMobType.ToString();
    }

    private void AddUpwardsImpulse()
    {
        GetComponent<Rigidbody2D>().AddForce(_impulseForce, ForceMode2D.Impulse);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Return()
    {
        OnReturnRequest?.Invoke(this);
    }
}
