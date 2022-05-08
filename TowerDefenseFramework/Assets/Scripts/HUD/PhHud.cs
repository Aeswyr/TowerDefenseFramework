using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhHud : MonoBehaviour
{
    [SerializeField] Destination moat;

    RectTransform gradient;
    RectTransform phIndicator;
    RectTransform rangeStart;
    RectTransform rangeEnd;
    Transform phText;

    // Start is called before the first frame update
    void Start()
    { 
        gradient = transform.Find("PH_Gradient").GetComponent<RectTransform>();
        phIndicator = gradient.Find("PH_Indicator").GetComponent<RectTransform>();
        rangeStart = gradient.Find("Range_Start").GetComponent<RectTransform>();
        rangeEnd = gradient.Find("Range_End").GetComponent<RectTransform>();
        phText = phIndicator.Find("PH_Text");
    }

    // Update is called once per frame
    void Update()
    {
        TMPro.TMP_Text tmp= phText.GetComponent<TMPro.TMP_Text>();
        tmp.text = moat.getPH().ToString("0.0");
        SetElementPosition(phIndicator, moat.getPH());
        SetElementPosition(rangeStart, moat.MinAcceptablePh);
        SetElementPosition(rangeEnd, moat.MaxAcceptablePh);
    }

    private void SetElementPosition(RectTransform element, float ph) {
        element.anchoredPosition = new Vector2((ph / 14f) * gradient.rect.width, element.anchoredPosition.y);
    }
}
