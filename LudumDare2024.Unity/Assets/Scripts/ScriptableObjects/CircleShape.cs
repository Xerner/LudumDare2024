using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Circle", menuName = "Shape/Circle")]
public class CircleShape : ShapeSO
{
    [SerializeField]
    float radius = 5;

    public override IEnumerable<Vector2> GenerateShape(IShapeGeneratorService shapeGeneratorService)
    {
        return shapeGeneratorService.GenerateCircle(radius);
    }
}
