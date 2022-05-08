#if UNITY_WEBGL && !UNITY_EDITOR
#define FIREBASE
#endif

using System.Runtime.InteropServices;
using UnityEngine;

namespace pHAnalytics
{
    public static class FirebaseUtil
    {

        // Game Start
        [DllImport("__Internal")]
        private static extern void FBGameStart();

        public static void GameStart()
        {
            #if FIREBASE
            FBGameStart();
            #endif
        }

        // Level Select
        [DllImport("__Internal")]
        private static extern void FBLevelSelect(string level);

        public static void LevelSelect(string level)
        {
            #if FIREBASE
            FBLevelSelect(level);
            #endif
        }

    }
}