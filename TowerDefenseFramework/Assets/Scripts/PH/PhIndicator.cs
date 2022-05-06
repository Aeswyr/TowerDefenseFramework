using UnityEngine;
using PhNarwahl.pH;

namespace PhNarwahl {

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PhContainer))]
    public class PhIndicator : MonoBehaviour {
        public static Color acidColor = Color.magenta;
        public static Color baseColor = Color.cyan;

        void Update()
        {
            PhContainer phContainer = gameObject.GetComponent<PhContainer>();
            float ph = phContainer.getPH();
            // Debug.Log("PH = " + ph);
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(acidColor, baseColor, ph/14f);
        }
    }
}