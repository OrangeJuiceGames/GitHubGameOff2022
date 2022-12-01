using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnDown, OnUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp?.Invoke();
    }
}
