using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTool : MonoBehaviour
{
    public enum Status
    {
        Normal = 0,
        Active = 1,
    }

    [Header("Pen Canvas")]
    [SerializeField] private PenCanvas penCanvas;
    [SerializeField] private Camera cam;

    [Header("Dots")]
    [SerializeField] private Transform dotParent;
    [SerializeField] private GameObject dotPrefab;

    [Header("Lines")]
    [SerializeField] private Transform lineParent;
    [SerializeField] private GameObject linePrefab;

    private LineController currentLine;

    public void Clear()
    {
        EndCurrentLine();
        foreach (Transform g in dotParent.GetComponentInChildren<Transform>())
            Destroy(g.gameObject);

        foreach (Transform g in lineParent.GetComponentInChildren<Transform>())
            Destroy(g.gameObject);
    }

    private void Start()
    {
        penCanvas.OnPenCanvasLeftClickEvent += AddDot;
        penCanvas.OnPenCanvasRightClickEvent += EndCurrentLine;
    }

    public void EndCurrentLine()
    {
        if (currentLine != null)
        {
            currentLine.SetStatus(Status.Normal);
            currentLine = null;
        }
    }

    private LineController InstantiateLine()
    {
        LineController line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity, lineParent).GetComponent<LineController>();
        line.OnLeftClickEvent += SetCurrentLine;
        return line;
    }

    public void CreateLine()
    {
        EndCurrentLine();
        currentLine = InstantiateLine();
        currentLine.SetStatus(Status.Active);
    }

    public void ToggleLoop()
    {
        if (currentLine != null)
        {
            currentLine.ToggleLoop();
        }
    }

    private void AddDot()
    {
        if (currentLine == null)
        {
            currentLine = InstantiateLine();
            currentLine.SetStatus(Status.Active);
        }

        DotController dot = Instantiate(dotPrefab, GetMousePosition(), Quaternion.identity, dotParent).GetComponent<DotController>();
        dot.OnDragEvent += MoveDot;
        dot.OnRightClickEvent += RemoveDot;
        dot.OnLeftClickEvent += DotLeftClick;
        currentLine.AddDot(dot, Status.Active);
    }

    private void DotLeftClick(DotController dot)
    {
        foreach (LineController line in dot.lines)
        {
            // if the current line is null
            // and the dot has a line, select the first line.
            if (currentLine == null && dot.lines.Count > 0)
            {
                SetCurrentLine(dot.lines[0]);
                break;
            }
            // if the line is not the currentLine,
            // add the dot to the currentLine
            else if (currentLine != line && currentLine && !currentLine.HasDot(dot))
            {
                currentLine.AddDot(dot, Status.Active);
                return;
            }

            // the line is the currentLine
            else if (currentLine.isLooped())
            {
                int currentIndex = dot.lines.IndexOf(currentLine);
                int newIndex = (currentIndex + 1) % dot.lines.Count;
                SetCurrentLine(dot.lines[newIndex]);
                break;
            }
            else
            {
                CloseLine(line);
                break;
            }

        }
    }

    private void CloseLine(LineController line)
    {
        EndCurrentLine();
        if (!line.isLooped()) line.ToggleLoop();
    }

    private void SetCurrentLine(LineController line)
    {
        EndCurrentLine();
        currentLine = line;
        currentLine.SetStatus(Status.Active);
    }

    private void MoveDot(DotController dot)
    {
        dot.transform.position = GetMousePosition();
    }

    private void RemoveDot(DotController dot)
    {
        List<LineController> linesToDel = new List<LineController>();

        foreach (LineController line in dot.lines)
        {
            line.RemoveDot(dot, out List<DotController> newDots);
            linesToDel.Add(line);

            if (newDots.Count == 0) break;

            LineController newLine = InstantiateLine();
            bool isActive = false;
            if (line == currentLine)
            {
                currentLine.SetStatus(Status.Normal);
                currentLine = newLine;
                currentLine.SetStatus(Status.Active);
                isActive = true;
            }

            if (line.isLooped()) newLine.ToggleLoop();

            for (int i = 0; i < newDots.Count; i++)
            {
                newLine.AddDot(newDots[i], isActive ? Status.Active : Status.Normal);
            }
        }

        Destroy(dot.gameObject);

        foreach (LineController line in linesToDel)
        {
            line.ClearSelf();
            Destroy(line.gameObject);
        }
    }

    private Vector3 GetMousePosition()
    {
        if (cam)
        {
            Vector3 worldMousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            worldMousePosition.z = 0;
            return worldMousePosition;
        }
        return Vector3.zero;
    }

}
