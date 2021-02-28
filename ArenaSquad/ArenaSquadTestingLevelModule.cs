using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using ThunderRoad;
using UnityEngine;
// ReSharper disable UnusedMember.Global

namespace ArenaSquad
{
    public class ArenaSquadTestingLevelModule : LevelModule
    {
        public override IEnumerator OnLoadCoroutine(Level level)
        {
            DebugLogConsole.AddCommandInstance("sw",
                "Start Wave", "StartWave",
                this);
            DebugLogConsole.AddCommandInstance("asd",
                "Toggle Arena Squad Data", "ArenaSquadData",
                this);
            return base.OnLoadCoroutine(level);
        }

        public void StartWave()
        {
            var waveData = Catalog.GetData<WaveData>("GladiatorEndlessMeleeOnly");
            List<SpawnLocation> spawnLocationList =
                new List<SpawnLocation>(Object.FindObjectsOfType<SpawnLocation>());

            var levelModuleWave = Level.current.modeRank.mode.GetModule<LevelModuleWave>();
            levelModuleWave.waveData = waveData;
            if (spawnLocationList.Count > 0)
                levelModuleWave.StartWave(spawnLocationList[0]);
            else
                Debug.LogError( "There is no spawnlocation on the map");
        }

        public void ArenaSquadData()
        {
            var data = GameManager.local.gameObject.GetComponent<ArenaSquadData>();

            data.data.isEnabled = !data.data.isEnabled;
        }
    }
}