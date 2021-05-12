using UnityEngine;

public class Pad : MonoBehaviour
{
    #region Variables

    [SerializeField] private SpriteRenderer spriteRenderer;

    private float horizontalLimit;
    private Ball ball;

    #endregion

    #region Unity lifecycle

    private void OnEnable()
    {
        Ball.OnCreate += HandleBallCreate;
    }

    private void OnDisable()
    {
        Ball.OnCreate -= HandleBallCreate;
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

    #endregion
}