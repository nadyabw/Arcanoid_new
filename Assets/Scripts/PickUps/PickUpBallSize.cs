using System;
using UnityEngine;

public class PickUpBallSize : BasePickUp
{
    [SerializeField] private float sizeFactor = 1.25f;

    public static event Action<PickUpBallSize> OnPickUpBallSizeCollected;

    public float SizeFactor
    {
        get => sizeFactor;
    }

    protected override void ApplyEffect()
    {
        OnPickUpBallSizeCollected?.Invoke(this);
    }
}