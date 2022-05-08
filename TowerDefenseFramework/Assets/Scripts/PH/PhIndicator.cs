using UnityEngine;
using PhNarwahl.pH;

namespace PhNarwahl {

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(HasPh))]
    public class PhIndicator : MonoBehaviour {
        [SerializeField] private float displayedPh = 7;

        public static Color acidColor = Color.green;
        public static Color neutralColor = Color.blue;
        public static Color baseColor = Color.red;

        public static Color GetColor(float ph) {
            if(ph > 7) {
                return Color.Lerp(neutralColor, baseColor, (ph-7)/7f);
            }
            return Color.Lerp(acidColor, neutralColor, ph/7);
        }

        void Update()
        {
            HasPh hasPh = gameObject.GetComponent<HasPh>();
            float ph = hasPh.getPH();
            displayedPh = ph;
            gameObject.GetComponent<SpriteRenderer>().color = GetColor(ph);
        }
    }
}