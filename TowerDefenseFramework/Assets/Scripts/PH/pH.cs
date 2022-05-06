
using UnityEngine;

namespace PhNarwahl {
    namespace pH {

        public interface PhContainer {
            float getPH();
            void MixSolution(float volume, float molH, float molOH);
        }

        public static class pH
        {
            
            public static float TOL = .000000001f;
            public static bool isAcidic(float ph) {
                return ph < 7 - TOL;
            } 

            public static bool isBasic(float ph) {
                return ph > 7 + TOL;
            }

            public static bool isNeutral(float ph) {
                return 7 - TOL < ph && ph < 7 + TOL;
            }

            public static float getAcidMolarity(float ph) {
                if (ph <= 0 || 7 <= ph) {
                    return 0;
                }
                return Mathf.Pow(10, -ph);
            }

            public static float getBaseMolarity(float ph) {
                if(ph <= 7 || 14 <= ph) {
                    return 0;
                }
                return Mathf.Pow(10, ph - 14);
            }

            public static float getPH(float volume, float molH, float molOH) {
                if (molH == molOH) {
                    return 7;
                }
                bool isAcidic = molH > molOH;
                if(isAcidic) {
                    return getAcidPH((molH - molOH)/volume);
                } else {
                    return 14 - getBasePOH((molOH - molH)/volume);
                }
            }

            public static float getAcidPH(float molarity) {
                return Mathf.Min(-Mathf.Log10(molarity), 7);
            }

            public static float getBasePOH(float molarity) {
                return Mathf.Min(-Mathf.Log10(molarity), 7);
            }
        }
    }
}