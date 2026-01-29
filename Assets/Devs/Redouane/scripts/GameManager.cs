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

    private GameObject playerObject;
    private GameState currentState;
    // but i want to grab it through game manager each time we swap scene 
    // the game object is called Player by name should i hardcode get it like that? 
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
        SceneManager.sceneLoaded += PlayerScene;
    }
    private void PlayerScene(Scene scene, LoadSceneMode mode)
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
        {
            Debug.LogWarning("Couldn't find player");
        }
    }


    private void Update()
    {

    }


}
