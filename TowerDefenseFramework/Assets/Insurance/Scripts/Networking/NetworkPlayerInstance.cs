using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class NetworkPlayerInstance : NetworkBehaviour
{
    //public NetworkVariable<bool> IsRequesting = new NetworkVariable<bool>();

    [SerializeField] private RequestMenu m_requestMenu;
    [SerializeField] private RequestReponseMenu m_requestResponseMenu;

    [SerializeField] private Button m_waitRoomContinueButton;

    private static int LEVEL_BUILD_INDEX = 5; // statically defined because SceneManager can't get index of unloaded scene

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            // TODO: debug why this appears for client when connected to host
            m_waitRoomContinueButton.gameObject.SetActive(true);
            m_waitRoomContinueButton.onClick.AddListener(HandleContinue);
        }

        NetworkSceneManager.Instance.CurrScene.OnValueChanged += new NetworkVariable<int>.OnValueChangedDelegate(HandleSceneChanged);
    }

    private void RecordPlayerArrival() {
        if (NetworkManager.Singleton.IsServer) {
            // removed
        }
        else {
            RecordPlayerArrivalRequestServerRpc();
        }
    }

    [ServerRpc]
    void RecordPlayerArrivalRequestServerRpc(ServerRpcParams rpcParams = default) {
        // removed
    }

    private void UpdatePlayerCount(ulong clientID) {
        WaitRoomManager.Instance.PlayerCount.Value = WaitRoomManager.Instance.PlayerCount.Value + 1;
        WaitRoomManager.Instance.PlayerCountText.text = WaitRoomManager.Instance.PlayerCount.Value + "/2 Players";
    }

    private void HandleContinue() {
        NetworkSceneManager.Instance.CurrScene.Value = LEVEL_BUILD_INDEX;
    }

    private void HandleSceneChanged(int prevVal, int currVal) {
        SceneManager.LoadScene(currVal);
    }


    private void Update() {

        /*
        if (NetworkRequestManager.instance.AnyPlayerRequesting.Value) {
            if (!m_requestResponseMenu.gameObject.activeSelf) {
                HandleIncomingRequest();
            }
        }
        else {
            // if menu is still open, close it
            if (m_requestResponseMenu.gameObject.activeSelf) {
                m_requestResponseMenu.Close();
            }
        }
        */
    }

    /*
    #region Own Request Menu

    void ActivateRequestMenu() {
        m_requestMenu.Open(HandleOwnRequestHelp);
    }

    public void HandleOwnRequestHelp() {
        //IsRequesting.Value = true;
        NetworkRequestManager.instance.AnyPlayerRequesting.Value = true;
    }

    void HandleOwnRequestResolved() {
        //IsRequesting.Value = false;
        NetworkRequestManager.instance.AnyPlayerRequesting.Value = false;
    }

    #endregion // Own Request Menu

    #region Incoming Request Response Menu

    void HandleIncomingRequest() {
        m_requestResponseMenu.Open(HandleIncomingHelpButton, HandleIncomingRefuseButton);
    }

    void HandleIncomingHelpButton() {
        m_requestResponseMenu.Close();
        //IsRequesting.Value = false;
        NetworkRequestManager.instance.AnyPlayerRequesting.Value = false;
    }

    void HandleIncomingRefuseButton() {
        m_requestResponseMenu.Close();
        //IsRequesting.Value = false;
        NetworkRequestManager.instance.AnyPlayerRequesting.Value = false;
    }

    #endregion // Incoming Request Reponse Menu

    */
}
