using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IShapeGeneratorService
{
    IEnumerable<Vector2> GenerateStar(int vertices, float radius = 1f);
    IEnumerable<Vector2> GenerateCircle(float radius = 1f);
    IEnumerable<Vector2> GenerateSquare(float radius = 1f);
}

public class ShapeGeneratorService : IShapeGeneratorService
{
    public const float twoPI = Mathf.PI * 2;
    public const float FullCircle = 360f;

    public float Radians(float degrees) => degrees * Mathf.PI / 180;

    public IEnumerable<Vector2> GenerateStar(int vertices, float radius = 1f)
    {
        var points = new List<Vector2>();
        var star = new List<Vector2>();
        for (float degrees = 0f; degrees < FullCircle; degrees += FullCircle / vertices)
        {
            points.Add(new Vector2(Mathf.Sin(Radians(degrees)), Mathf.Cos(Radians(degrees))) * radius);
        }
        for (int i = 0; star.Count < points.Count; i+=2)
        {
            star.Add(points[i % points.Count]);
        }
        // close the star
        star.Add(points[0]);
        return star;
    }

    public IEnumerable<Vector2> GenerateCircle(float radius = 1f)
    {
        var circle = new List<Vector2>();
        for (float degrees = 0f; degrees < FullCircle; degrees++)
        {
            var point = new Vector2(Mathf.Sin(Radians(degrees)), Mathf.Cos(Radians(degrees))) * radius;
            circle.Add(point);
        }
        // close the circle
        circle.Add(circle[0]);
        return circle;
    }

    public IEnumerable<Vector2> GenerateSquare(float radius = 1f)
    {
        var square = new List<Vector2>
        {
            new Vector2(-radius, -radius),
            new Vector2(-radius, radius),
            new Vector2(radius, radius),
            new Vector2(radius, -radius),
            new Vector2(-radius, -radius)
        };
        return square;
    }
}
