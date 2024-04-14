using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Star", menuName = "Shape/Star")]
public class StarShape : ShapeSO
{
    [SerializeField]
    int vertices = 5;
    [SerializeField]
    float radius = 5;

    public override IEnumerable<Vector2> GenerateShape(IShapeGeneratorService shapeGeneratorService)
    {
        return shapeGeneratorService.GenerateStar(vertices, radius);
    }
}
