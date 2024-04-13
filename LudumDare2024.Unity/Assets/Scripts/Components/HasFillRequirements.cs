using System;
using Unity.Collections;
using UnityEngine;

public class HasFillRequirements : MonoBehaviour
{
    public float targetAmount;
    public float acceptableRange;
    public SpriteRenderer spillOverSprite;

    [ReadOnly]
    public float currentAmount;

    public void Start()
    {
        spillOverSprite.enabled = false;
        transform.position = new Vector3(transform.position.x, currentAmount, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, currentAmount * 2f, transform.localScale.z);
    }

    public void Increment(float amount)
    {
        if (IsPastRange())
        {
            spillOverSprite.enabled = true;
            return;
        }

        currentAmount += amount;
        transform.position = new Vector3(transform.position.x, currentAmount, transform.position.z);
        transform.localScale = new Vector3(transform.localScale.x, currentAmount * 2f, transform.localScale.z);
    }

    public bool IsWithinRange()
    {
        return Math.Abs(targetAmount - currentAmount) <= Math.Abs(acceptableRange);
    }

    public bool IsPastRange()
    {
        return currentAmount > (targetAmount + Math.Abs(acceptableRange));
    }
}
