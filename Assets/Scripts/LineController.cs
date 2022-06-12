using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineController : MonoBehaviour, IPointerClickHandler
{

    [Header("Colors")]
    [SerializeField] private Color activeColor;
    [SerializeField] private Color normalColor;

    public Action<LineController> OnLeftClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnLeftClickEvent?.Invoke(this);
    }


    private LineRenderer lr;
    private List<DotController> dots;
    public MeshGenerator mg;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        mg = GetComponent<MeshGenerator>();
        lr.positionCount = 0;
        dots = new List<DotController>();
    }

    public void AddDot(DotController dot, PenTool.Status s)
    {
        dot.SetStatus(s);
        dot.AddLine(this);

        lr.positionCount++;
        dots.Add(dot);
        // mg.UpdateShape();
    }

    public bool HasDot(DotController dot)
    {
        return dots.Contains(dot);
    }

    public void ToggleLoop()
    {
        lr.loop = !lr.loop;
        if (lr.loop) mg.UpdateShape();
    }

    public bool isLooped()
    {
        return lr.loop;
    }

    private void SetColor(Color c)
    {
        lr.startColor = c;
        lr.endColor = c;
    }

    public void SetStatus(PenTool.Status s)
    {
        foreach (DotController dot in dots)
        {
            dot.SetStatus(s);
        }
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

    public void RemoveDot(DotController dot, out List<DotController> newDots)
    {
        newDots = new List<DotController>();
        int index = dots.IndexOf(dot);

        int i = 0;
        for (; i < index; i++)
        {
            newDots.Add(dots[i]);
        }
        i++;
        for (; i < dots.Count; i++)
        {
            newDots.Add(dots[i]);
        }
    }

    public void ClearSelf()
    {
        foreach (DotController dot in dots)
        {
            dot.lines.Remove(this);
        }
    }

    private void LateUpdate()
    {
        if (dots.Count >= 2)
        {
            for (int i = 0; i < dots.Count; i++)
            {
                lr.SetPosition(i, dots[i].transform.position);
            }
        }
    }

    public Vector3[] GetVertices()
    {
        Vector3[] positions = new Vector3[lr.positionCount];
        lr.GetPositions(positions);
        return positions;
    }
}
