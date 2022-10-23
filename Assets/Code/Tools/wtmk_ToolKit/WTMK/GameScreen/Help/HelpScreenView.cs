using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    private GameObject _UI;
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.SetActive(isActive);
    }
}
