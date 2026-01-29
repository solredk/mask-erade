using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
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


    public void QuitGame()
    {
        Application.Quit();
    }
}
