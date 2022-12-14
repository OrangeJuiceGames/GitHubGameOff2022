using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShipMovement
{
    public event Action<Vector3> OnReachStop;

    private float _stopLength;
    private float _remainingStopWait;
    private bool isAtStop = false, _IsSpawning;

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

    public void SpawnComplete()
    {
        _IsSpawning = false;
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

        if(_IsSpawning)
        {
            return _currentPos;
        }

        var newPos = Vector3.MoveTowards(_currentPos, _targetPos, _movementSpeed * Time.deltaTime);
        _currentPos = newPos;
        
        return newPos;
    }

    public void UpdateMoveSpeed(float newMovementSpeed)
    {
        _movementSpeed = newMovementSpeed;
    }

    public void UpdateStopLength(float newStopLength)
    {
        if (_remainingStopWait > 0)
        {
            float difference = newStopLength - _stopLength;
            _remainingStopWait = _remainingStopWait + difference >= 0 ? _remainingStopWait + difference : 0;
        }
        _stopLength = newStopLength;
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
        if(_IsSpawning)
        {
            return;
        }

        _IsSpawning = true;
        OnReachStop?.Invoke(_currentPos);
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
