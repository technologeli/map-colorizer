using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PenCanvas : MonoBehaviour, IPointerClickHandler
{

    public Action OnPenCanvasLeftClickEvent;
    public Action OnPenCanvasRightClickEvent;

    public void OnPointerClick(PointerEventData eventData)
    {
        // left click
        if (eventData.pointerId == -1)
        {
            OnPenCanvasLeftClickEvent?.Invoke();
        }
        else if (eventData.pointerId == -2)
        {
            OnPenCanvasRightClickEvent?.Invoke();
        }
    }

}
