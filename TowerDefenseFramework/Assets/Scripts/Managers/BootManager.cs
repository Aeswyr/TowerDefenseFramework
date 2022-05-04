using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using pHAnalytics;

public class BootManager : MonoBehaviour
{
    private void Start() {
        FirebaseUtil.GameStart();
        SceneManager.LoadScene("MainMenu");
    }
}
