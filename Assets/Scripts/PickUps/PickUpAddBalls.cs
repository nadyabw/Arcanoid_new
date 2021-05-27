using System;
using UnityEngine;

public class PickUpAddBalls : BasePickUp
{
    [SerializeField] private int ballsNumberToAdd = 2;

    public static event Action<PickUpAddBalls> OnPickUpAddBallsCollected;

    public float BallsNumberToAdd { get => ballsNumberToAdd; }

    protected override void ApplyEffect()
    {
        OnPickUpAddBallsCollected?.Invoke(this);
    }
}