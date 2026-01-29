using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject PauseCanvas;

    public bool isPaused = false;

    public void PauseGame()
    {
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            PauseCanvas.SetActive(false);            
            Cursor.visible = false;
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            PauseCanvas.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0f;
            isPaused = true;            
        }
    }

    public void Loadscene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
