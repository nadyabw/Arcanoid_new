using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    #region Variables

    [Header("UI")] [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text scoreText;

    #endregion

    #region Events

    public static event Action OnClose;

    #endregion

    #region Unity lifecycle

    void Start()
    {
        scoreText.text = $"Total score: {Game.TotalScore}";
        playAgainButton.onClick.AddListener(PlayAgainClickHandler);
        exitButton.onClick.AddListener(ExitClickHandler);
    }

    #endregion

    #region Event Handlers

    private void PlayAgainClickHandler()
    {
        OnClose?.Invoke();
    }

    private void ExitClickHandler()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    #endregion
}