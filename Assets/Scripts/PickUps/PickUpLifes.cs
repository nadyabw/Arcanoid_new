using System;
using UnityEngine;

public class PickUpLifes : BasePickUp
{
    [SerializeField] private bool isLifeAdd;
    public static event Action<PickUpLifes> OnPickUpLifeCollected;
    public bool IsLifeAdd
    {
        get => isLifeAdd;
    }

    protected override void ApplyEffect()
    {
        OnPickUpLifeCollected?.Invoke(this);
    }
}