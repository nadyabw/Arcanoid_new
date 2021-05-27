using System;
using UnityEngine;

public class PickUpPadWidth : BasePickUp
{
    [SerializeField] private float widthFactor = 1.25f;

    public static event Action<PickUpPadWidth> OnPickUpPadWidthCollected;

    public float WidthFactor
    {
        get => widthFactor;
    }

    protected override void ApplyEffect()
    {
        OnPickUpPadWidthCollected?.Invoke(this);
    }
}