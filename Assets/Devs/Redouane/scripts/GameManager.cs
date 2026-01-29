using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager GameInstance { get; private set; }

    private GameState currentState;

    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        if (GameInstance != null && GameInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        GameInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
    }

    private void Update()
    {
<<<<<<< Updated upstream
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No more levels. Game complete!");
            return;
        }

        SceneManager.LoadScene(nextIndex);
    }

    public void LoadLevel(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Invalid level index: " + buildIndex);
            return;
        }

        SceneManager.LoadScene(buildIndex);
    }

    public void Pause()
    {
        currentState = GameState.Paused;
        if (pauseScreen != null)
            pauseScreen.SetActive(true);
    }

    public void Died()
    {
        currentState = GameState.GameOver;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
=======

    }


}
>>>>>>> Stashed changes
