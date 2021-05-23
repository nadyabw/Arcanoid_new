using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    #region Variables

    [SerializeField] private LifesView lifesView; // вьюшка жизней
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
        isAutoplay = settings.isAutoplayMode; // считываем режим игры из настроек

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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        // код для добавления жизней
        if (Input.GetKeyDown(KeyCode.F))
        {
           AddLife(1);
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
        PauseView.OnClose += HandlePauseClose;
        GameOverView.OnClose += HandleGameOverClose;
        PickUpScore.OnPickUpScoreCollected += HandlePickUpScoreDownCollected;
        PickUpLifes.OnPickUpLifeCollected += HandlePickUpLifeCollected;
    }

    private void RemoveHandlers()
    {
        Block.OnCreate -= HandleBlockCreate;
        Block.OnDestroy -= HandleBlockDestroy;
        Floor.onBallLoss -= HandleBallLoss;
        PauseView.OnClose -= HandlePauseClose;
        GameOverView.OnClose -= HandleGameOverClose;
        PickUpScore.OnPickUpScoreCollected -= HandlePickUpScoreDownCollected;
        PickUpLifes.OnPickUpLifeCollected += HandlePickUpLifeCollected;
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

    private void HandleBlockDestroy(Block block)
    {
        blocksNumberToFinish--;
        AddScore(block.ScoreForDestroy);

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
        AddScore(psd.Score);
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

    public void AddScore(int value)
    {
        totalScore += value;
        UpdeteScoreText();
    }

    private void UpdeteScoreText()
    {
        textScore.text = $"Score: {totalScore}";
    }
    
    private void HandlePickUpLifeCollected(PickUpLifes plu)
    {
        if (plu.IsLifeAdd == true)
            AddLife(1);
        else
        {
            AddLife(-1);
        }
        
    }

    private void AddLife(int lifesNum)
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