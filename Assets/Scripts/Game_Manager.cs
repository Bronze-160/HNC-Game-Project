using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public void StartButton()
    {
        Time.timeScale = 1f;
        // SceneManager.LoadScene("Level_1");
        SceneManager.LoadSceneAsync("Level_1");
    }
    public void RestartButton()
    {
        Time.timeScale = 1f;
        Debug.Log("Restart");
        SceneManager.LoadSceneAsync("Start_Level");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
