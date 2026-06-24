using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadLevel (int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    System.Collections.IEnumerator LoadSceneAsync (int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone) 
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            
            yield return null;
        }
        

    }
}
