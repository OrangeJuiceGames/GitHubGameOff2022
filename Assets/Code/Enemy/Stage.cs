using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField]
    private Transform _LeftWall, _RightWall, _Roof;
    [SerializeField]
    private TextMeshProUGUI _Level, _InvasionTime, _UID, _Wave;
    [SerializeField]
    private Image _ExpBar;

    public MobileInput MobileInput;
    public Animator ScrapomaticEffect;

    public Floor Floor => _Floor;
    public ShipFactory ShipFactory => _ShipFactory;
    public PlayerController Player => _Player;
    public ScoreView ScoreView => _ScoreView;
    public Boss Boss => _Boss;
    public Transform LeftWall => _LeftWall;
    public Transform RightWall => _RightWall;
    public Transform Roof => _Roof;
    public Image ExpBar => _ExpBar;
    public TextMeshProUGUI Level => _Level;
    public TextMeshProUGUI InvasionTime => _InvasionTime;
    public TextMeshProUGUI UID => _UID;
    public TextMeshProUGUI Wave => _Wave;
}
