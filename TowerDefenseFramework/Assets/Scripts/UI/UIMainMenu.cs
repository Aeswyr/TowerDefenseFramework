using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainMenu : MenuBase {
    #region Editor

    [SerializeField]
    private Button m_newGameButton;
    [SerializeField]
    private Button m_quitButton;

    #endregion

    #region Unity Callbacks

    private void Awake() {
        m_newGameButton.onClick.AddListener(HandleNewGame);
        m_quitButton.onClick.AddListener(HandleQuit);
    }

    #endregion

    #region ButtonHandlers

    private void HandleNewGame() {
        SceneManager.LoadScene("SampleScene"); // change to whichever scene is your next
        AudioManager.instance.PlayOneShot("menu-click-default");
    }

    private void HandleQuit() {
        Application.Quit();
        AudioManager.instance.PlayOneShot("menu-click-default");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    #endregion
}
