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

    public ShipController BuidShip()
    {
        var clone = Instantiate(_ShipPrefab);
        clone.transform.SetParent(_StageRoot);
        clone.transform.position = _SpawnPoint.position;
        return clone.GetComponent<ShipController>();
    }
}
