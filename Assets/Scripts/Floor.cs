using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Floor : MonoBehaviour
{
    public static event Action<GameObject> onBallLoss; 
    private void OnCollisionEnter2D(Collision2D other)
    {
        onBallLoss?.Invoke(other.gameObject);
        Destroy(other.gameObject);
    }
}
