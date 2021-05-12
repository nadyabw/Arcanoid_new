using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    #region Variables

    [Header("UI")]

    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    #endregion

    #region UnityLifecycle
    private void Start()
    {
        playButton.onClick.AddListener(PlayClickHandler);
        exitButton.onClick.AddListener(ExitClickHandler);
    }

    #endregion

    #region EventHandlers

    private void PlayClickHandler()
    {
        SceneManager.LoadScene(1);
    }

    private void ExitClickHandler()
    {
        Application.Quit();
    }

    #endregion
}
