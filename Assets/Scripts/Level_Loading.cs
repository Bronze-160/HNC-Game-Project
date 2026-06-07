using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Loading : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider Slider;
    public TMP_Text progressText;
    public void LoadLevel(int sceneIndex )
    {
        
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            Debug.Log(progress);
            Slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null; 
        }
    }
}
