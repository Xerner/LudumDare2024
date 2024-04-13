using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreBehaviour : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI linesText;
    [SerializeField]
    TextMeshProUGUI scoreText;

    ILineScoringService lineScoringService;

    [Inject]
    public void Construct(ILineScoringService lineScoringService)
    {
        this.lineScoringService = lineScoringService;
    }

    void OnValidate()
    {
        if (linesText == null)
        {
            Debug.LogError("Lines text is not set.", this);
        }
        if (scoreText == null)
        {
            Debug.LogError("Score text is not set.", this);
        }
    }

    public void UpdateScore()
    {
        linesText.text = $"Lines: {lineScoringService.GetLines()}";
        scoreText.text = $"Score: {lineScoringService.GetScore()}";
    }
}
