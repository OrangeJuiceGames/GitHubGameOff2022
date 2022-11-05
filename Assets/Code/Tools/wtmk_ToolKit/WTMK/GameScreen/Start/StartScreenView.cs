using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    protected Button _Start;
    public Button bStart => _Start;
    [SerializeField]
    private Canvas _UI;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
    }
}
