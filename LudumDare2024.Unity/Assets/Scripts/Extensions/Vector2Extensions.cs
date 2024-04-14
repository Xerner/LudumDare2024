using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Vector2Extensions
{
    public static Vector2 AddScalar(this Vector2 vector3, float scalar)
    {
        return new Vector2(vector3.x + scalar, vector3.y + scalar);
    }

    /// <summary>
    /// https://stackoverflow.com/questions/39853481/is-point-inside-polygon
    /// </summary>
    public static bool IsInPolygon(this Vector2 testPoint, IEnumerable<Vector2> vertices)
    {
        if (vertices.Count() < 3) return false;
        bool isInPolygon = false;
        var lastVertex = vertices.ElementAt(vertices.Count() - 1);
        foreach (var vertex in vertices)
        {
            if (testPoint.y.IsBetween(lastVertex.y, vertex.y))
            {
                double t = (testPoint.y - lastVertex.y) / (vertex.y - lastVertex.y);
                double x = t * (vertex.x - lastVertex.x) + lastVertex.x;
                if (x >= testPoint.x) isInPolygon = !isInPolygon;
            }
            else
            {
                if (testPoint.y == lastVertex.y && testPoint.x < lastVertex.x && vertex.y > testPoint.y) isInPolygon = !isInPolygon;
                if (testPoint.y == vertex.y && testPoint.x < vertex.x && lastVertex.y > testPoint.y) isInPolygon = !isInPolygon;
            }

            lastVertex = vertex;
        }

        return isInPolygon;
    }
}
