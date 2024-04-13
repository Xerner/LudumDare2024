using System.Collections.Generic;
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
    LineRenderer currentLineRenderer;
    public bool IsDrawing { get; set; }

    ILineScoringService lineScoringService;

    [Inject]
    public void Construct(ILineScoringService lineScoringService)
    {
        this.lineScoringService = lineScoringService;
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
    }

    void Start()
    {
        lineScoringService.Reset();
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
        drawAction.action.canceled += StopDrawing;
    }

    void UnsubscribeFromInput()
    {
        drawAction.action.started -= StartDrawing;
        drawAction.action.canceled -= StopDrawing;
    }

    public void StartDrawing(InputAction.CallbackContext ctx)
    {
        currentLineRenderer = Instantiate(lineRendererPrefab, transform);
        ResetLine(currentLineRenderer);
        lineScoringService.LineRenderers.Add(currentLineRenderer);
        IsDrawing = true;
    }
    public void StopDrawing(InputAction.CallbackContext ctx)
    {
        IsDrawing = false;
    }

    public void ResetLine(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }

    void DrawLine()
    {
        var mousePosition = Mouse.current.position.ReadValue(); 
        worldPositon = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
        //Debug.Log($"Mouse position: {mousePosition} \tWorld position: {worldPositon}");
        worldPositon.z = 0f;
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, worldPositon);
    }
}
