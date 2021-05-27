using System;
using UnityEngine;

public class PickUpLifes : BasePickUp
{
    [SerializeField] private int lifesNumber = 1;
    public static event Action<PickUpLifes> OnPickUpLifeCollected;

    public int LifesNumber
    {
        get => lifesNumber;
    }

    protected override void ApplyEffect()
    {
        OnPickUpLifeCollected?.Invoke(this);
    }
}