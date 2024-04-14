using System.Collections.Generic;
using UnityEngine;

public interface ILineScoringService
{
    List<LineRenderer> LineRenderers { get; }
    List<LineRenderer> ExpectedLines { get; }
    int LineCount { get; }
    float AverageDistance { get; }
    void Reset();
    int GetLines();
    string GetGradeLetter();
    string GradeToLetter(int grade);
}

public class LineScoringService : ILineScoringService
{
    List<LineRenderer> Lines { get; set; } = new List<LineRenderer>();
    List<LineRenderer> ExpectedLines { get; set; } = new List<LineRenderer>();

    List<LineRenderer> ILineScoringService.LineRenderers => Lines;
    List<LineRenderer> ILineScoringService.ExpectedLines => ExpectedLines;
    public int LineCount { get; private set; } = 0;
    public float AverageDistance { get; private set; } = 0f;

    public int GetLines()
    {
        LineCount = Lines.Count;
        return LineCount;
    }

    public string GetGradeLetter()
    {
        return GradeToLetter(CalculateGrade());
    }

    public void Reset()
    {
        Lines.Clear();
    }

    int CalculateGrade()
    {
        var linesCount = GetLines();
        var averageDistance = GetAveragePointDistance();
        //var drawnPointsToExpectedPointsRatio = GetDrawnPointsToExpectedPointsRatio();
        var grade = 0;
        if (linesCount > 0 && linesCount <= ExpectedLines.Count * 2)
        {
            grade += 1;
        }
        if (averageDistance > 0f && averageDistance < 1.5f)
        {
            grade += 1;
        }
        if (averageDistance > 0f && averageDistance < 1f)
        {
            grade += 1;
        }
        if (averageDistance > 0f && averageDistance < 0.5f)
        {
            grade += 1;
        }
        //if (drawnPointsToExpectedPointsRatio > 0.5f || drawnPointsToExpectedPointsRatio < 1.5f)
        //{
        //    grade += 1;
        //}
        return grade;
    }

    public string GradeToLetter(int grade)
    {
        if (grade == 4)
        {
            return "A";
        }
        if (grade == 3)
        {
            return "B";
        }
        if (grade == 2)
        {
            return "C";
        }
        if (grade == 1)
        {
            return "C";
        }
        return "F";
    }

    float GetAveragePointDistance()
    {
        AverageDistance = 0;
        var drawnPointsCount = 0;
        var allExpectedPoints = CombinePoints(ExpectedLines);
        foreach (var line in Lines)
        {
            for (int i = 0; i < line.positionCount; i++)
            {
                var point = line.GetPosition(i);
                var closestDistance = DistanceOfClosestPoint(point, allExpectedPoints);
                AverageDistance += closestDistance;
            }
            drawnPointsCount += line.positionCount;
        }
        if (drawnPointsCount == 0)
        {
            AverageDistance = 0f;
            return 0f;
        }
        AverageDistance /= drawnPointsCount;
        return AverageDistance;
    }

    float DistanceOfClosestPoint(Vector2 point, IEnumerable<Vector2> points)
    {
        var closestDistance = float.MaxValue;
        foreach (var p in points)
        {
            var distance = Vector2.Distance(point, p);
            if (distance < closestDistance)
            {
                closestDistance = distance;
            }
        }
        return closestDistance;
    }

    //float GetDrawnPointsToExpectedPointsRatio()
    //{
    //    var drawnPointsCount = 0;
    //    var expectedPointsCount = 0;
    //    foreach (var line in Lines)
    //    {
    //        drawnPointsCount += line.positionCount;
    //    }
    //    foreach (var line in ExpectedLines)
    //    {
    //        expectedPointsCount += line.positionCount;
    //    }
    //    if (expectedPointsCount == 0)
    //    {
    //        return 0;
    //    }
    //    PointsRatio = (float)drawnPointsCount / expectedPointsCount;
    //    return PointsRatio;
    //}

    List<Vector2> CombinePoints(IEnumerable<LineRenderer> lines)
    {
        var combinedPoints = new List<Vector2>();
        foreach (var line in lines)
        {
            for (int i = 0; i < line.positionCount; i++)
            {
                combinedPoints.Add(line.GetPosition(i));
            }
        }
        return combinedPoints;
    }
}
