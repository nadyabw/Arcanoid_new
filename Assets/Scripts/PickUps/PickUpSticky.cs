using System;

public class PickUpSticky : BasePickUp
{
    public static event Action<PickUpSticky> OnPickUpStickyCollected;

    protected override void ApplyEffect()
    {
        OnPickUpStickyCollected?.Invoke(this);
    }
}