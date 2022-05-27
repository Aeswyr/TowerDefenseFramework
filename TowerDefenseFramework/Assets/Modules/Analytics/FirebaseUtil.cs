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

        //Return to Level Select
        [DllImport("__Internal")]
        private static extern void FBLeaveLevel(string level);

        public static void LeaveLevel(string level)
        {
            #if FIREBASE
            FBLeaveLevel(level);
            #endif
        }

        //Oncomer Spawned
        [DllImport("__Internal")]
        private static extern void FBOncomerSpawned(string level, string oncomerName);

        public static void OncomerSpawned(string level, string oncomerName)
        {
            #if FIREBASE
            FBOncomerSpawned(level, oncomerName);
            #endif
        }

        //Tower Placed
        [DllImport("__Internal")]
        private static extern void FBTowerPlaced(string level, string position, string towerName);

        public static void TowerPlaced(string level, Vector3 position, string towerName)
        {
            #if FIREBASE
            FBTowerPlaced(level, position.ToString(), towerName);
            #endif
        }

        //Oncomer Hit Moat
        [DllImport("__Internal")]
        private static extern void FBHitMoat(string level, string oncomerName, float oncomerPH, float oncomerVolume, float moatPH, float moatVolume);

        public static void HitMoat(string level, string oncomerName, float oncomerPH, float oncomerVolume, float moatPH, float moatVolume)
        {
            #if FIREBASE
            FBHitMoat(level, oncomerName, oncomerPH, oncomerVolume, moatPH, moatVolume);
            #endif
        }

        //End Level - Final Moat pH, 
        [DllImport("__Internal")]
        private static extern void FBLevelEnd(string level, float pH, float volume);

        public static void LevelEnd(string level, float pH, float volume)
        {
            #if FIREBASE
            FBLevelEnd(level, pH, volume);
            #endif
        }

    }
}