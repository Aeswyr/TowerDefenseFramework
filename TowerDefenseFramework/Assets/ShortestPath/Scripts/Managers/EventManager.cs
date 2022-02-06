using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityEvent OnGridUpdate;

    private void Awake() {
        OnGridUpdate = new UnityEvent();
    }
}
