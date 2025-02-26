using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptMenu : MonoBehaviour
{
    public Slider slider;
    public GameObject Load;
    public Text progressText;
    public void PlayGame(int sceneIndex)
    {

        StartCoroutine(LoadAsynchronously(sceneIndex));
        /*while (progress != 1f)
        {
            progress += 0.05f;
            slider.value = progress;
            Debug.Log(progress);
        }
        if(progress == 1)
        {
            SceneManager.LoadScene("SALY");
        }*/
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        Load.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100 + " %";

            yield return null;
        }
    }

    public void Exit()
    {
        Debug.Log("End");
        Application.Quit();
    }
}
