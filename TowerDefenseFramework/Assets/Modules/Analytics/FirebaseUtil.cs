#if UNITY_WEBGL && !UNITY_EDITOR
#define FIREBASE
#endif

using System.Runtime.InteropServices;
using UnityEngine;

namespace pHAnalytics
{
    public static class FirebaseUtil
    {

        #region Firebase JS Functions

        //Init
        [DllImport("__Internal")]
        private static extern void FBGameStart();

        //Progression
        [DllImport("__Internal")]
        private static extern void FBLevelSelect(string level);

        #endregion // Firebase JS Functions

        #region Log Events
        public static void GameStart()
        {
            Debug.Log("FirebaseUtil: Game Start - 0");
            #if FIREBASE
            Debug.Log("FirebaseUtil: Game Start - 1");
            FBGameStart();
            #endif
        }

        public static void LevelSelect(string level)
        {
            Debug.Log("FirebaseUtil: Level Select - 0");
            #if FIREBASE
            Debug.Log("FirebaseUtil: Level Select - 1");
            FBLevelSelect(level);
            #endif
        }
        #endregion // Log Events
    }
}