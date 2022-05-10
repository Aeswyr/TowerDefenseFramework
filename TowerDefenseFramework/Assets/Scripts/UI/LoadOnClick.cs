using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour
{
    public void LoadSceneByName(string sceneName) {
        try {
            SceneManager.LoadScene(sceneName);
        } catch (Exception) {
            Debug.Log("Failed to load scene with name: " + sceneName);
        }
    }
}
