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
    private Stage _Stage;
    [SerializeField]
    private Story _Story;

    public PlayerController Player => _Player;
    public Stage Stage => _Stage;
    public Story Story => _Story;

    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
        _Player.gameObject.SetActive(true);
        _Stage.gameObject.SetActive(true);
    }
}
