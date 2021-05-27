using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    #region Variables
    [Header("Base settings")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float startOffsetY = 0.6f;
    [SerializeField] private Pad pad;
    [SerializeField] private TrailRenderer trailRenderer;
    [Header("Random Direction")]
    [SerializeField] private Vector2 minRandomDirection = new Vector2(-1f, 1f);
    [SerializeField] private Vector2 maxRandomDirection = new Vector2(0.85f, 1f);
    
    [Header("Speed settings")]
    [SerializeField] private float gameSpeed;
    [SerializeField] private float minSpeed = 6f;
    [SerializeField] private float maxSpeed = 12f;

    [Header("Size settings")]
    [SerializeField] private float minSizeFactor = 0.5f;
    [SerializeField] private float maxSizeFactor = 1.5f;

    private float XOffsetFromPadCentre;
    private bool isStarted;

    #endregion

    #region Events

    public static event Action<Ball> OnCreate;

    #endregion

    #region Unity lifecycle
    private void OnEnable()
    {
        PickUpSpeed.OnPickUpSpeedCollected += HandlePickUpSpeed;
        PickUpBallSize.OnPickUpBallSizeCollected += HandlePickUpSize;
      
    }

    private void OnDisable()
    {
        PickUpSpeed.OnPickUpSpeedCollected -= HandlePickUpSpeed;
        PickUpBallSize.OnPickUpBallSizeCollected -= HandlePickUpSize;
      
    }

    private void Start()
    {
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
            Vector3 pos = new Vector3(pad.transform.position.x + XOffsetFromPadCentre, pad.transform.position.y + startOffsetY, 0);
            transform.position = pos;
            
            Debug.Log(pos.y);

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

    public void StartBall()
    {
        isStarted = true;
        trailRenderer.enabled = true;

        float dirX = Random.Range(minRandomDirection.x, maxRandomDirection.x);
        float dirY = Random.Range(minRandomDirection.y, maxRandomDirection.y);
        rb.velocity = (new Vector2(dirX, dirY).normalized) * gameSpeed;
       
    }
    private void ChangeSize(float sizeFactor)
    {
        Vector3 size = transform.localScale;
        size.x *= sizeFactor;
        size.x = Mathf.Clamp(size.x, minSizeFactor, maxSizeFactor);
        size.y = size.x;
        transform.localScale = size;
    }
    private void ChangeSpeed(float speedFactor)
    {
        gameSpeed *= speedFactor;
        gameSpeed = Mathf.Clamp(gameSpeed, minSpeed, maxSpeed);
        rb.velocity = gameSpeed * (rb.velocity.normalized);
    }
    private void StopBall()
    {
        isStarted = false;
        trailRenderer.enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        XOffsetFromPadCentre = transform.position.x - pad.transform.position.x;
    }
 
    #endregion
    
    #region Event handlers
    private void HandlePickUpSpeed(PickUpSpeed ps)
    {
        ChangeSpeed(ps.SpeedFactor);
    }
    private void HandlePickUpSize(PickUpBallSize pbs)
    {
        ChangeSize(pbs.SizeFactor);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Pad))
        {
            if(pad.IsSticky)
            {
                StopBall();
            }
        }
    }
    #endregion
}