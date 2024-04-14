using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class LineDrawerBehaviour : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRendererPrefab;
    [SerializeField] 
    InputActionReference drawAction;
    Vector3 worldPositon;
    LineRenderer currentLineRenderer = null;

    public bool IsDrawing { get; set; }

    ILineScoringService lineScoringService;
    IShapeGeneratorService shapeGeneratorService;

    [SerializeField]
    List<ShapeSO> ExpectedShapes = new();
    [SerializeField]
    ShapeSO DrawingBoundaryShape;
    [SerializeField]
    float DrawingBoundaryDrawWidth = 0.1f;
    IEnumerable<Vector2> DrawingBoundaries = new List<Vector2>();
    [SerializeField]
    float ExpectedShapesZOffset = 0f;
    [SerializeField]
    bool canDraw = false;

    [Inject]
    public void Construct(ILineScoringService lineScoringService, IShapeGeneratorService shapeGeneratorService)
    {
        this.lineScoringService = lineScoringService;
        this.shapeGeneratorService = shapeGeneratorService;
    }

    void OnValidate()
    {
        if (drawAction == null)
        {
            Debug.LogError("Draw action is not set.", this);
        }
        if (lineRendererPrefab == null)
        {
            Debug.LogError("Line renderer prefab is not set.", this);
        }
        if (ExpectedShapes == null || ExpectedShapes.Count == 0)
        {
            Debug.LogError("Expected shapes are not set.", this);
        }
        if (DrawingBoundaryShape == null)
        {
            Debug.LogError("Drawing boundary shape is not set.", this);
        }
    }

    void Start()
    {
        lineScoringService.Reset();
        StartCoroutine(DrawShapes(ExpectedShapes, ExpectedShapesZOffset));
        SubscribeToInput();
    }

    void OnDestroy()
    {
        UnsubscribeFromInput();
    }

    void Update()
    {
        //Debug.Log($"Drawing: {IsDrawing} Mouse Position: {worldPositon}");
        if (IsDrawing)
        {
            DrawLine();
        }
    }

    void SubscribeToInput()
    {
        drawAction.action.started += StartDrawing;
        drawAction.action.performed += StartDrawing;
        drawAction.action.canceled += StopDrawing;
    }

    void UnsubscribeFromInput()
    {
        drawAction.action.started -= StartDrawing;
        drawAction.action.canceled -= StopDrawing;
    }

    public void StartDrawing(InputAction.CallbackContext ctx)
    {
        IsDrawing = true;
    }

    public void StopDrawing(InputAction.CallbackContext ctx)
    {
        currentLineRenderer = null;
        IsDrawing = false;
    }

    public void ResetLine(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }

    void DrawLine()
    {
        if (!canDraw)
        {
            return;
        }
        var mousePosition = Mouse.current.position.ReadValue(); 
        worldPositon = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
        worldPositon.z = 0f;

        if (DrawingBoundaries.Any())
        {
            if (!new Vector2(worldPositon.x, worldPositon.y).IsInPolygon(DrawingBoundaries))
            {
                return;
            }
        }
        if (currentLineRenderer is null)
        {
            currentLineRenderer = Instantiate(lineRendererPrefab, transform);
            ResetLine(currentLineRenderer);
            lineScoringService.LineRenderers.Add(currentLineRenderer);
        }
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, worldPositon);
    }

    IEnumerator DrawShapes(IEnumerable<ShapeSO> shapes, float zOffset)
    {
        foreach (var shape in shapes)
        {
            yield return StartCoroutine(DrawWithWait(shape, zOffset));
        }
        yield return StartCoroutine(DrawWithWait(DrawingBoundaryShape, zOffset, 0.25f, DrawingBoundaryDrawWidth));
        DrawingBoundaries = DrawingBoundaryShape.GenerateShape(shapeGeneratorService);
        canDraw = true;
    }

    IEnumerator DrawWithWait(ShapeSO shape, float zOffset, float timeToCompleteEntireShape = 1f, float width = 0f)
    {
        var shapeLineRenderer = Instantiate(lineRendererPrefab, transform);
        if (width > 0f)
        {
            shapeLineRenderer.startWidth = width;
            shapeLineRenderer.endWidth = width;
        }
        shapeLineRenderer.colorGradient = shape.Gradient;
        shapeLineRenderer.name = shape.name;
        var points = shape.GenerateShape(shapeGeneratorService);
        var maxPosition = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        var vector3Points = points.Select(p => new Vector3(p.x, p.y, zOffset));
        var timeToCompleteSegment = timeToCompleteEntireShape / points.Count();
        Vector3? previousPoint = null;
        foreach (var point in vector3Points)
        {
            if (point.x > maxPosition.x)
            {
                maxPosition.x = point.x;
            }
            if (point.y > maxPosition.y)
            {
                maxPosition.y = point.y;
            }

            if (previousPoint.HasValue)
            {
                shapeLineRenderer.positionCount++;
                shapeLineRenderer.SetPosition(shapeLineRenderer.positionCount - 1, previousPoint.Value);
                yield return MoveLinePointOverTime(shapeLineRenderer, point, shapeLineRenderer.positionCount - 1, timeToCompleteSegment);
                previousPoint = point;
                continue;
            }
            shapeLineRenderer.positionCount++;
            shapeLineRenderer.SetPosition(shapeLineRenderer.positionCount - 1, point);
            previousPoint = point;
            yield return new WaitForSeconds(timeToCompleteSegment);
        }
        lineScoringService.ExpectedLines.Add(shapeLineRenderer);
    }

    IEnumerator MoveLinePointOverTime(LineRenderer line, Vector3 destination, int index, float timeToComplete)
    {
        var startPosition = line.GetPosition(index);
        var timeElapsed = 0f;
        while (timeElapsed < timeToComplete)
        {
            timeElapsed += Time.deltaTime;
            line.SetPosition(index, Vector3.Lerp(startPosition, destination, timeElapsed / timeToComplete));
            yield return null;
        }
    }
}
