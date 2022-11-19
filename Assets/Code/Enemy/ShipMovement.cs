using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement
{
    private Vector3 _topPosition;
    private bool _isMovingLeft = true;
    private float _movementSpeed;

    private Vector3 _currentPos;


    public ShipMovement(Vector3 topPosition, float movementSpeed)
    {
        _topPosition = topPosition;
        _currentPos = topPosition;
        _movementSpeed = movementSpeed;
    }

    public Vector3 GetNewPosition()
    {
        if (_isMovingLeft)
        {
            _currentPos.x -= Time.deltaTime * _movementSpeed;
        }
        else
        {
            _currentPos.x += Time.deltaTime * _movementSpeed;
        }
        
        return _currentPos;
    }

    public void ChangeDirection()
    {
        _isMovingLeft = !_isMovingLeft;
    }



}
