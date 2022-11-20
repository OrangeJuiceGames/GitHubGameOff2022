using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipMovement
{
    public event Action OnReachStop;

    private float _stopLength;
    private float _remainingStopWait;
    private bool isAtStop = false;

    private Vector3 _currentPos;
    private float _movementSpeed;
    private Vector3 _targetPos;

    private List<ShipPath> _paths;
    private int _pathIndex;
    private Queue<Vector3> _stops;

    public ShipMovement(float movementSpeed, float stopLength, Vector3 currentPos, List<ShipPath> shipPaths)
    {
        _movementSpeed = movementSpeed;
        _currentPos = currentPos;
        _paths = shipPaths;
        _stops = GetNextSetOfStops();
        _stopLength = stopLength;

        _targetPos = GetNextPosition();
    }

    public Vector3 MoveTowardsStop()
    {
        if (isAtStop)
        {
            CountDownStop();
            return _currentPos;
        }

        if (_currentPos == _targetPos)
        {
            StartStopTimer();
            InvokeStop();
            _targetPos = GetNextPosition();

            return _currentPos;
        }

        var newPos = Vector3.MoveTowards(_currentPos, _targetPos, _movementSpeed * Time.deltaTime);
        _currentPos = newPos;

        return newPos;
    }

    private void StartStopTimer()
    {
        isAtStop = true;
        _remainingStopWait = _stopLength;
    }

    private void CountDownStop()
    {
        _remainingStopWait -= Time.deltaTime;
        if (_remainingStopWait <= 0) { isAtStop = false; }
    }

    private void InvokeStop()
    {
        OnReachStop?.Invoke();
    }

    private Vector3 GetNextPosition()
    {
        if (_stops.Count == 0)
        {
            _stops = GetNextSetOfStops();
        }

        return _stops.Dequeue();
    }

    private Queue<Vector3> GetNextSetOfStops()
    {
        _pathIndex++;
        if (_pathIndex > _paths.Count - 1)
        {
            _pathIndex = 0;
        }

        return _paths[_pathIndex].GetPath();
    }



}
