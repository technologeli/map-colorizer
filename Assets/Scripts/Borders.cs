using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Borders
{
    public static bool hasBorder(Vector3[] r1, Vector3[] r2)
    {
        // in order for two regions to border,
        // two consecutive points (in either order) must be the same.
        HashSet<HashSet<Vector3>> consecutives1 = GetConsecutives(r1);
        HashSet<HashSet<Vector3>> consecutives2 = GetConsecutives(r2);

        foreach (HashSet<Vector3> c1 in consecutives1)
            foreach (HashSet<Vector3> c2 in consecutives2)
                if (consecutiveEquals(c1, c2))
                    return true;
        return false;
    }

    private static bool consecutiveEquals(HashSet<Vector3> c1, HashSet<Vector3> c2)
    {
        if (c1.Count != c2.Count) return false;
        // should only be two in each set, so maximum of four iterations

        foreach (Vector3 v1 in c1)
        {
            bool hasV = false;
            foreach (Vector3 v2 in c2) if (v1.Equals(v2)) hasV = true;
            if (!hasV) return false;
        }
        return true;
    }

    private static HashSet<HashSet<Vector3>> GetConsecutives(Vector3[] r)
    {
        HashSet<HashSet<Vector3>> consecutives = new HashSet<HashSet<Vector3>>();

        // loop case
        HashSet<Vector3> loop = new HashSet<Vector3>();
        loop.Add(r[0]);
        loop.Add(r[r.Length - 1]);
        consecutives.Add(loop);

        for (int i = 0; i < r.Length - 1; i++)
        {
            HashSet<Vector3> s = new HashSet<Vector3>();
            s.Add(r[i]);
            s.Add(r[i + 1]);
            consecutives.Add(s);
        }
        return consecutives;
    }
}
