using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    private Canvas _UI;
    [SerializeField]
    private PlayerController _Player;
    [SerializeField]
    private GameObject _Stage;
    [SerializeField]
    private Floor _Floor;

    public PlayerController Player => _Player;
    public Floor Floor => _Floor;

    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
        _Player.gameObject.SetActive(true);
        _Stage.SetActive(true);
    }
}
