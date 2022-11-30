using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreenView : MonoBehaviour, IStateView
{
    [SerializeField]
    protected Button _Start;
    public Button bStart => _Start;
    public Button Credits;
    [SerializeField]
    private Canvas _UI;
    public TextMeshProUGUI Leaderboard_Names;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _UI.gameObject.SetActive(isActive);
    }
}
