using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInsuranceMenu : MenuBase
{
    [SerializeField] private Button m_purchaseButton;

    void OnEnable() {
        m_purchaseButton.onClick.AddListener(HandlePurchase);
    }

    void OnDisable() {
        m_purchaseButton.onClick.RemoveAllListeners();
    }

    void HandlePurchase() {
        base.CloseMenu();
        EventManager.OnPurchaseInsuranceComplete.Invoke();
    }
}
