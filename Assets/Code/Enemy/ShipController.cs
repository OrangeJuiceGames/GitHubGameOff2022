using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] SpriteRenderer _model;
    private ShipMovement _shipMovement;

    // Start is called before the first frame update
    void Start()
    {
        _shipMovement = new ShipMovement(this.transform.position, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _shipMovement.GetNewPosition();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "RightBlocker" || collision.gameObject.name == "LeftBlocker")
        {
            _shipMovement.ChangeDirection();
        }
    }

}
