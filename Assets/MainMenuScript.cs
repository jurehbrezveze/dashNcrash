using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        Debug.Log("MainMenuScript is running!");

        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
            Debug.Log("Play Button listener added.");
        }
        else
        {
            Debug.LogError("Play Button is not assigned in the Inspector.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            Debug.Log("Quit Button listener added.");
        }
        else
        {
            Debug.LogError("Quit Button is not assigned in the Inspector.");
        }
    }

    private void PlayGame()
    {
        Debug.Log("Loading next scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
        Debug.Log("Quitting the game...");
        Application.Quit();
    }
}
