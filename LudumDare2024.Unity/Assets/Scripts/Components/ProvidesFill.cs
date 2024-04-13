using System;
using UnityEngine;

public class ProvidesFill : MonoBehaviour
{
    public HasFillRequirements target;
    public float fillRate;
    public int maxFillRateMultiplier;

    private int spaceDownFrameCount;

    public void Fill()
    {
        target.Increment(fillRate * ((spaceDownFrameCount / 100) * Time.deltaTime));
    } 

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceDownFrameCount = Math.Min(++spaceDownFrameCount, maxFillRateMultiplier * 100);
        }
        else
        {
            spaceDownFrameCount = Math.Max(0, spaceDownFrameCount - 10);
        }

        Fill();
    }
}
