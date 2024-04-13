using System.Collections.Generic;
using UnityEngine;

public interface ILineScoringService
{
    List<LineRenderer> LineRenderers { get; }
    List<LineRenderer> ExpectedLines { get; }
    void Reset();
    int GetLines();
    int GetScore();
}

public class LineScoringService : ILineScoringService
{
    public LineScoringService()
    {

    }

    List<LineRenderer> Lines { get; set; } = new List<LineRenderer>();
    List<LineRenderer> ExpectedLines { get; set; } = new List<LineRenderer>();

    List<LineRenderer> ILineScoringService.LineRenderers => Lines;
    List<LineRenderer> ILineScoringService.ExpectedLines => ExpectedLines;

    public int GetLines()
    {
        return Lines.Count;
    }

    public int GetScore()
    {
        int score = 0;
        return score;
    }

    public void Reset()
    {
        Lines.Clear();
    }
}
