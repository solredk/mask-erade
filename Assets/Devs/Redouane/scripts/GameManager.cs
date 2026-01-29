using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private GameState currentState;

    [SerializeField] private GameObject pauseScreen;

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    private void Pause()
    {
        currentState = GameState.Paused;
        pauseScreen.SetActive(true);
    }

    private void Died()
    {
        currentState = GameState.GameOver;
    }
}
