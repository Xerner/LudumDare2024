using System.Collections.Generic;
using UnityEngine;

public abstract class ShapeSO : ScriptableObject
{
    [SerializeField]
    Gradient gradient;

    public Gradient Gradient => gradient;
    public abstract IEnumerable<Vector2> GenerateShape(IShapeGeneratorService shapeGeneratorService);
}
