using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreBehaviour : MonoBehaviour
{
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
        if (scoreText == null)
        {
            Debug.LogError("Score text is not set.", this);
        }
    }

    void Start()
    {
        UpdateScore();
    }

    public void UpdateScore()
    {
        var gradeLetter = lineScoringService.GetGradeLetter();
        scoreText.text = $@"Lines: {lineScoringService.LineCount}
Average Distance: {lineScoringService.AverageDistance}
Score: {gradeLetter}";
    }
}
