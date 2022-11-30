using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScreenView : MonoBehaviour, IStateView
{
    public GameObject UI;
    public Button Exit;

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        UI.SetActive(isActive);
    }
}
