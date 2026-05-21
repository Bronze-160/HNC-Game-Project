using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public void StartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level_3");
    }
    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start_Level");
    }

}
