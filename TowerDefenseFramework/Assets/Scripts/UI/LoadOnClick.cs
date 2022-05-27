using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using pHAnalytics;

public class LoadOnClick : MonoBehaviour
{
    public void LeaveLevel(string sceneName) {
        var currentScene = SceneManager.GetActiveScene().name;
        FirebaseUtil.LeaveLevel(currentScene);
        LoadSceneByName(sceneName);
    }

    public void LoadSceneByName(string sceneName) {
        try {
            SceneManager.LoadScene(sceneName);
        } catch (Exception) {
            Debug.Log("Failed to load scene with name: " + sceneName);
        }
    }
}
