using System;
using System.Collections.Generic;
using UnityEngine;

namespace PhNarwahl {

    public static class NarwhalUtils {
       
        public static C BestGameObjectBy<C>(
            List<GameObject> elements, 
            Func<C, C, bool> isBetter
        ) {
            bool foundAnyBest = false;
            C best = default(C);
            foreach (GameObject e in elements) {
                C component = e.GetComponent<C>();
                if (component != null && (!foundAnyBest || isBetter(component, best))) {
                    foundAnyBest = true;
                    best = component;
                }
            }
            return best;
        }
    }
}
