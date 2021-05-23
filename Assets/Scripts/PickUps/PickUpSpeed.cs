using System;
using UnityEngine;

public class PickUpSpeed : BasePickUp
{
    [SerializeField] private float speedFactor = 1.2f;

    public static event Action<PickUpSpeed> OnPickUpSpeedCollected;

    public float SpeedFactor { get => speedFactor;}

    protected override void ApplyEffect()
    {
        OnPickUpSpeedCollected?.Invoke(this);
    }
}