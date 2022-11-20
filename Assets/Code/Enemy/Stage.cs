using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private ShipFactory _ShipFactory;
    [SerializeField]
    private Floor _Floor;
    public Floor Floor => _Floor;
}
