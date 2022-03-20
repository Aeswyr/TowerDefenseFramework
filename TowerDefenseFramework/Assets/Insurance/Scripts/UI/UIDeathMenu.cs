using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDeathMenu : MenuBase {
    [SerializeField] Button m_returnButton;

    void OnEnable() {
        m_returnButton.onClick.AddListener(HandleReturnLevelSelect);
    }

    void OnDisable() {
        m_returnButton.onClick.RemoveAllListeners();
    }

    public void Open() {
        base.OpenMenu();
    }

    void HandleReturnLevelSelect() {
        base.CloseMenu();
        AudioManager.instance.StopAudio();
        SceneManager.LoadScene("LevelSelect");
    }
}