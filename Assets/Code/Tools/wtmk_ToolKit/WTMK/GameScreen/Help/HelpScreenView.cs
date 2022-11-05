using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreenView : MonoBehaviour, IStateView
{
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
