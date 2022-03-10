using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkInterfacer : NetworkBehaviour
{
    public NetworkVariable<bool> IsRequesting = new NetworkVariable<bool>();

    [SerializeField] private RequestMenu m_requestMenu;
    [SerializeField] private RequestReponseMenu m_requestResponseMenu;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            IsRequesting.Value = false;
        }
    }

    public void HandleOwnRequestHelp() {
        IsRequesting.Value = true;
    }

    void HandleOwnRequestResolved() {
        IsRequesting.Value = true;
    }

    #region Own Request Menu

    void ActivateRequestMenu() {
        m_requestMenu.Open(HandleOwnRequestHelp);
    }

    #endregion // Own Request Menu

    #region Incoming Request Response Menu

    void HandleIncomingRequest() {
        m_requestResponseMenu.Open(HandleIncomingHelpButton, HandleIncomingRefuseButton);
    }

    void HandleIncomingHelpButton() {
        m_requestResponseMenu.Close();
    }

    void HandleIncomingRefuseButton() {
        m_requestResponseMenu.Close();
    }

    #endregion // Incoming Request Reponse Menu
}
