using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Square", menuName = "Shape/Square")]
public class SquareShape : ShapeSO
{
    [SerializeField]
    float radius = 1;

    public override IEnumerable<Vector2> GenerateShape(IShapeGeneratorService shapeGeneratorService)
    {
        return shapeGeneratorService.GenerateSquare(radius);
    }
}
