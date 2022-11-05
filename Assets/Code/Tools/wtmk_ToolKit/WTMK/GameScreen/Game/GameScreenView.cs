using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    private Canvas _UI;

    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
    }
}
