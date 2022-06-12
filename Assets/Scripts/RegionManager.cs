using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material defaultMat;
    [SerializeField] private Material mat1;
    [SerializeField] private Material mat2;
    [SerializeField] private Material mat3;
    [SerializeField] private Material mat4;
    private Material[] possibleMats;

    private void Awake()
    {
        possibleMats = new Material[] { mat1, mat2, mat3, mat4 };
    }

    private LineController[] lines;
    private List<LineController>[] allRegionBorders;

    private void GenerateRegionBorders()
    {
        // every region has a list of regions that border it
        // and every region has an index. therefore, we can construct
        // an array of Lists of regions (jagged array)

        // must be generated every time
        lines = GetComponentsInChildren<LineController>();

        List<LineController>[] regions = new List<LineController>[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            regions[i] = new List<LineController>();

            Vector3[] r1 = lines[i].GetVertices();

            for (int j = 0; j < lines.Length; j++)
            {
                // skip when they are the same
                if (i == j) continue;

                Vector3[] r2 = lines[j].GetVertices();
                if (Borders.hasBorder(r1, r2))
                {
                    regions[i].Add(lines[j]);
                }
            }
        }

        allRegionBorders = regions;
    }

    public void ColorRegions()
    {
        GenerateRegionBorders();

        // first, uncolor all of them
        foreach (LineController line in lines)
        {
            line.GetComponent<MeshRenderer>().material = defaultMat;
        }

        ColorRegionsRecursive();
    }

    // recursive is private because it requires some other code, but that
    // code is expensive and should only be run once
    private bool ColorRegionsRecursive()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            MeshRenderer mr = lines[i].GetComponent<MeshRenderer>();

            // if the region is not blank skip checking, since the part below will handle it
            // when all of the regions have an assigned color, this will continue every time
            // so it will return true
            if (!isBlank(mr.material.name)) continue;

            foreach (Material mat in possibleMats)
            {
                if (possible(i, mat.name))
                {
                    mr.material = mat;
                    bool done = ColorRegionsRecursive();
                    if (done) return true;
                    mr.material = defaultMat;
                }
            }
            return false;
        }

        bool allAssigned = true;
        for (int i = 0; i < lines.Length; i++)
        {
            MeshRenderer mr = lines[i].GetComponent<MeshRenderer>();
            if (isBlank(mr.material.name))
                allAssigned = false;
        }

        if (allAssigned)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                MeshRenderer mr = lines[i].GetComponent<MeshRenderer>();
            }
            return true;
        }
        return false;
    }

    private string MaterialName(string mn)
    {
        return mn.Split(' ')[0];
    }

    private bool isBlank(string mn)
    {
        return defaultMat.name.Equals(MaterialName(mn));
    }

    // https://en.wikipedia.org/wiki/Four_color_theorem
    private bool possible(int i, string mn)
    {
        // keep track of every color that appears in a set
        HashSet<string> matNames = new HashSet<string>();

        // go through all of region's borders and add to set
        foreach (LineController line in allRegionBorders[i])
            matNames.Add(MaterialName(line.GetComponent<MeshRenderer>().material.name));

        return !matNames.Contains(mn);

    }
}
