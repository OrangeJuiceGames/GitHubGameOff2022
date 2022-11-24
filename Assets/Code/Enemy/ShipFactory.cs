using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFactory : MonoBehaviour
{
    [SerializeField]
    private ShipController _ShipPrefab;
    [SerializeField]
    private Transform _StageRoot;
    [SerializeField]
    private Transform _SpawnPoint;

    private float _startingStopLength = 0.7f;
    private float _startingSpeed = 2f;
    private float _speedIncrement = 0.1f;
    private float _stopDecrement = 0.005f;

    public ShipController BuidShip(int waveNum)
    {
        var clone = Instantiate(_ShipPrefab);
        clone.transform.SetParent(_StageRoot);
        clone.transform.position = _SpawnPoint.position;
        
        ShipController ship = clone.GetComponent<ShipController>();

        float shipSpeed = GetShipSpeed(waveNum);
        float shipStopLength = GetShipStopLength(waveNum);
        ship.SetShipStats(shipSpeed, shipStopLength);

        return ship;
    }

    private float GetShipSpeed(int waveNum)
    {
        return _startingSpeed + (_speedIncrement * waveNum);
    }

    private float GetShipStopLength(int waveNum)
    {
        return _startingStopLength - (_stopDecrement * waveNum);
    }



}
