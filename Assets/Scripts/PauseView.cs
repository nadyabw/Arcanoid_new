using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    #region Variables

    [Header("UI")]

    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;

    #endregion

    #region Events

    public static event Action OnClose;

    #endregion

    #region Unity lifecycle

    void Start()
    {
        continueButton.onClick.AddListener(ContinueClickHandler);
        exitButton.onClick.AddListener(ExitClickHandler);
    }

    #endregion

    #region Event Handlers

    private void ContinueClickHandler()
    {
        OnClose?.Invoke();
    }

    private void ExitClickHandler()
    {
        Application.Quit();
    }

    #endregion
}
