using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public string nextSceneName = "MainMenu";

    void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3); // Simule un temps de chargement
        SceneManager.LoadScene(nextSceneName);
    }
}
