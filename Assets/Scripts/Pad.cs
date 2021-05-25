using UnityEngine;

public class Pad : MonoBehaviour
{
    #region Variables

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float minWidthFactor = 0.5f;
    [SerializeField] private float maxWidthFactor = 1.5f;

    private float horizontalLimit;
    private Ball ball;
    private bool isSticky = false;
    public bool IsSticky { get => isSticky;}


    #endregion

    #region Unity lifecycle

    private void OnEnable()
    {
        Ball.OnCreate += HandleBallCreate;
        PickUpPadWidth.OnPickUpPadWidthCollected += HandlePickUpWidthCollected;
    }

    private void OnDisable()
    {
        Ball.OnCreate -= HandleBallCreate;
        PickUpPadWidth.OnPickUpPadWidthCollected -= HandlePickUpWidthCollected;
    }

    private void Start()
    {
        Vector2 screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float padWidthWorld = spriteRenderer.bounds.size.x;

        horizontalLimit = screenSizeWorld.x - padWidthWorld / 2;
    }

    private void Update()
    {
        if (Game.IsPaused)
        {
            return;
        }

        UpdatePosition();
    }

    #endregion

    #region Private methods

    private void UpdatePosition()
    {
        Vector3 padPos;
        if (Game.IsAutoplay) // следуем по Х за мячом
        {
            padPos = new Vector3(ball.transform.position.x, transform.position.y, 0);
        }
        else // следуем по Х за мышью
        {
            Vector3 pixelsPos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(pixelsPos);

            padPos = new Vector3(worldPos.x, transform.position.y, 0);
        }
        padPos.x = Mathf.Clamp(padPos.x, -horizontalLimit, horizontalLimit);
        transform.position = padPos;
    }

    private void HandleBallCreate(Ball b)
    {
        ball = b;
    }
    private void HandlePickUpStickyCollected(PickUpSticky ps)
    {
        isSticky = true;
    }
    private void HandlePickUpWidthCollected(PickUpPadWidth pw)
    {
        ChangeWidth(pw.WidthFactor);
    }
    private void ChangeWidth(float widthFactor)
    {
        Vector3 size = transform.localScale;
        size.x *= widthFactor;
        size.x = Mathf.Clamp(size.x, minWidthFactor, maxWidthFactor);
        transform.localScale = size;

        CalcHorizontalLimit();
    }
    private void CalcHorizontalLimit()
    {
        Vector2 screenSizeWorld = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float padWidthWorld = spriteRenderer.bounds.size.x;

        horizontalLimit = screenSizeWorld.x - padWidthWorld / 2;
    }

    #endregion
}