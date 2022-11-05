using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreenView : MonoBehaviour, IStateView
{
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
