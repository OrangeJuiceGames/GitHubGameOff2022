using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private ShipFactory _ShipFactory;
    [SerializeField]
    private Floor _Floor;
    [SerializeField]
    private PlayerController _Player;
    [SerializeField]
    private ScoreView _ScoreView;
    [SerializeField]
    private Boss _Boss;

    public Floor Floor => _Floor;
    public ShipFactory ShipFactory => _ShipFactory;
    public PlayerController Player => _Player;
    public ScoreView ScoreView => _ScoreView;
    public Boss Boss => _Boss;

}
