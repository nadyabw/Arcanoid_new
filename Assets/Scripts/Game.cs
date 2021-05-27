using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    #region Variables

    [SerializeField] private LifesView lifesView;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private GameObject pauseViewPrefab;
    [SerializeField] private GameObject gameOverPrefab;
    [SerializeField] private Text textScore;
    [SerializeField] private GameSettings settings;

    private GameObject pauseView;

    private int blocksNumberToFinish = 0;

    private static int lifesLeft = 0;
    private static int totalScore = 0;
    private static bool isPaused;
    private static bool isAutoplay;

    private List<Ball> activeBalls = new List<Ball>();

    #endregion

    #region Properties

    public static bool IsPaused => isPaused;
    public static int TotalScore => totalScore;
    public static bool IsAutoplay => isAutoplay;

    #endregion

    #region Unity lifecycle

    private void Start()
    {
        isPaused = false;
        isAutoplay = settings.isAutoplayMode;

        if (lifesLeft == 0)
        {
            lifesLeft = settings.lifesTotal;
            totalScore = 0;
        }

        UpdeteScoreText();
        lifesView.InitLifes(lifesLeft, settings.lifesTotal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // код для добавления жизней
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeLife(1);
        }
    }

    private void OnEnable()
    {
        AddHandlers();
    }

    private void OnDisable()
    {
        RemoveHandlers();
    }

    #endregion

    #region Private methods

    private void AddHandlers()
    {
        Block.OnCreate += HandleBlockCreate;
        Block.OnDestroy += HandleBlockDestroy;
        Floor.onBallLoss += HandleBallLoss;
        Ball.OnCreate += HandleBallCreated;
        PauseView.OnClose += HandlePauseClose;
        GameOverView.OnClose += HandleGameOverClose;
        PickUpScore.OnPickUpScoreCollected += HandlePickUpScoreDownCollected;
        PickUpLifes.OnPickUpLifeCollected += HandlePickUpLifeCollected;
        PickUpAddBalls.OnPickUpAddBallsCollected += HandlePickUpAddBallsCollected;
    }

    private void RemoveHandlers()
    {
        Block.OnCreate -= HandleBlockCreate;
        Block.OnDestroy -= HandleBlockDestroy;
        Ball.OnCreate -= HandleBallCreated;
        Floor.onBallLoss -= HandleBallLoss;
        PauseView.OnClose -= HandlePauseClose;
        GameOverView.OnClose -= HandleGameOverClose;
        PickUpScore.OnPickUpScoreCollected -= HandlePickUpScoreDownCollected;
        PickUpLifes.OnPickUpLifeCollected += HandlePickUpLifeCollected;
        PickUpAddBalls.OnPickUpAddBallsCollected -= HandlePickUpAddBallsCollected;
    }

    private void HandleGameOverClose()
    {
        SceneManager.LoadScene(0);
    }

    private void HandlePauseClose()
    {
        TogglePause();
    }

    private void HandleBallLoss(GameObject ball)
    {
        bool isAllBallsLost = RemoveActiveBall(ball);

        if (isAllBallsLost)
        {
            Debug.Log(lifesLeft);
            
            lifesLeft--;
            lifesView.UpdateLifes(lifesLeft);

            if (lifesLeft > 0)
            {
                Instantiate(ball);
            }
            else
            {
                isPaused = true;
                Instantiate(gameOverPrefab, canvasTransform);
            }
        }
    }

    private void HandleBallCreated(Ball b)
    {
        activeBalls.Add(b);
    }

    private bool RemoveActiveBall(GameObject ball)
    {
        for (int i = 0; i < activeBalls.Count; i++)
        {
            if (activeBalls[i].gameObject == ball)
            {
                activeBalls.RemoveAt(i);
                break;
            }
        }

        return activeBalls.Count == 0;
    }

    private void HandleBlockDestroy(Block block)
    {
        blocksNumberToFinish--;
        ChangeScore(block.ScoreForDestroy);

        if (blocksNumberToFinish == 0)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void HandlePickUpScoreDownCollected(PickUpScore psd)
    {
        ChangeScore(psd.Score);
    }

    private void HandleBlockCreate(bool isUndestroyable)
    {
        if (!isUndestroyable)
        {
            blocksNumberToFinish++;
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseView = Instantiate(pauseViewPrefab, canvasTransform);
        }
        else
        {
            Time.timeScale = 1f;
            Destroy(pauseView);
        }
    }

    private void HandlePickUpAddBallsCollected(PickUpAddBalls pab)
    {
        Ball ball;

        foreach (var b in activeBalls)
        {
            for (int i = 0; i < pab.BallsNumberToAdd; i++)
            {
                ball = Instantiate(b);
                ball.StartBall();
            }
        }
    }

    public void ChangeScore(int value)
    {
        totalScore += value;
        if (totalScore < 0)
        {
            totalScore = 0;
        }

        UpdeteScoreText();
    }

    private void UpdeteScoreText()
    {
        textScore.text = $"Score: {totalScore}";
    }

    private void HandlePickUpLifeCollected(PickUpLifes plu)
    {
        ChangeLife(plu.LifesNumber);
    }

    private void ChangeLife(int lifesNum)
    {
        lifesLeft += lifesNum;
        if (lifesLeft > settings.lifesTotal)
        {
            lifesLeft = settings.lifesTotal;
        }
        else if (lifesLeft <= 0)
        {
            isPaused = true;
            Instantiate(gameOverPrefab, canvasTransform);
        }

        lifesView.UpdateLifes(lifesLeft);
    }

    #endregion
}