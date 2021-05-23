using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BasePickUp : MonoBehaviour
{
    #region Unity lifecycle

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Pad))
        {
            ApplyEffect();
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag(Tags.Floor))
        {
            Destroy(gameObject);
        }
    }

    #endregion


    #region Private methods

   protected abstract void ApplyEffect();
  
    #endregion
}

