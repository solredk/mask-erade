using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnStartClicked()
    {
        GameManager.GameInstance.LoadLevel(1); // 0=MainMenu, 1=Tutorial 
    }
    public void OnQuitClicked()
    {
        GameManager.GameInstance.QuitGame();
    }

}
