using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShipPath")]
public class ShipPath : ScriptableObject
{
    
    [SerializeField] GameObject PathPrefab;

    public Queue<Vector3> GetPath()
    {
        Queue<Vector3> transforms = new Queue<Vector3>();

        foreach(Transform child in PathPrefab.transform)
        {
            transforms.Enqueue(child.position);
        }

        return transforms;
    }

}
