using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DotController : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public List<LineController> lines;
    private bool isDrag;

    [Header("Colors")]
    [SerializeField] private Color activeColor;
    [SerializeField] private Color normalColor;
    private Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public Action<DotController> OnDragEvent;
    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        foreach (LineController line in lines)
        {
            if (line.isLooped()) line.mg.UpdateShape();
        }
        OnDragEvent?.Invoke(this);
    }

    public Action<DotController> OnRightClickEvent;
    public Action<DotController> OnLeftClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        // Right click
        if (eventData.pointerId == -2)
        {
            OnRightClickEvent?.Invoke(this);
        }
        // Left click
        else if (eventData.pointerId == -1)
        {
            // Don't do anything if it was also a drag event
            if (isDrag)
            {
                isDrag = false;
                return;
            }

            OnLeftClickEvent?.Invoke(this);
        }

    }

    public void AddLine(LineController line)
    {
        lines.Add(line);
    }

    private void SetColor(Color c)
    {
        if (!img) img = GetComponent<Image>();
        img.color = c;
    }

    public void SetStatus(PenTool.Status s)
    {
        switch (s)
        {
            case PenTool.Status.Active:
                SetColor(activeColor);
                break;
            case PenTool.Status.Normal:
                SetColor(normalColor);
                break;
        }
    }
}
