
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetworkSceneManager : NetworkBehaviour
{
    public static NetworkSceneManager Instance;

    [HideInInspector]
    public NetworkVariable<int> CurrScene = new NetworkVariable<int>();

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (this != Instance) {
            //Destroy(this.gameObject);
            return;
        }
    }
}
