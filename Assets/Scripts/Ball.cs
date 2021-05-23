using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    #region Variables
    [Header("Base settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float startForceValue = 350f;
    [SerializeField] private float startOffsetY = -3.1f;
    
    [SerializeField] private Transform padTransform;
    [SerializeField] private TrailRenderer trailRenderer;
    [Header("Random Direction")]
    [SerializeField] private Vector2 minRandomDirection = new Vector2(-1f, 1f);
    [SerializeField] private Vector2 maxRandomDirection = new Vector2(0.85f, 1f);
    
    [Header("Speed settings")]
    [SerializeField] private float gameSpeed;
    [SerializeField] private float minSpeed = 6f;
    [SerializeField] private float maxSpeed = 12f;

    private bool isStarted;

    #endregion

    #region Events

    public static event Action<Ball> OnCreate;

    #endregion

    #region Unity lifecycle
    private void OnEnable()
    {
        PickUpSpeed.OnPickUpSpeedCollected += HandlePickUpSpeed;
      
    }

    private void OnDisable()
    {
        PickUpSpeed.OnPickUpSpeedCollected -= HandlePickUpSpeed;
      
    }

    private void Start()
    {
        trailRenderer.enabled = false;

        OnCreate?.Invoke(this);
    }

    private void Update()
    {
        if (Game.IsPaused)
        {
            return;
        }

        if (!isStarted)
        {
            Vector3 padPosition = padTransform.position;
            padPosition.y = startOffsetY;

            transform.position = padPosition;

            if(Game.IsAutoplay) // запускаем мяч автоматически
            {
                StartBall();
            }
            else if (Input.GetMouseButtonDown(0)) // запускаем мяч по клику мыши
            {
                StartBall();
            }
        }
    }

    #endregion



    #region Private methods
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(isStarted)
        {
            rb.velocity = gameSpeed * (rb.velocity.normalized);
        }
    }

    private void StartBall()
    {
        isStarted = true;
        trailRenderer.enabled = true;

        float dirX = Random.Range(minRandomDirection.x, maxRandomDirection.x);
        float dirY = Random.Range(minRandomDirection.y, maxRandomDirection.y);

        Vector2 force = (new Vector2(dirX, dirY).normalized) * startForceValue;
        rb.AddForce(force);
    }
    private void ChangeSpeed(float speedFactor)
    {
        gameSpeed *= speedFactor;
        gameSpeed = Mathf.Clamp(gameSpeed, minSpeed, maxSpeed);
    }
    
 
    #endregion
    
    #region Event handlers
    private void HandlePickUpSpeed(PickUpSpeed ps)
    {
        ChangeSpeed(ps.SpeedFactor);
    }
    #endregion
}