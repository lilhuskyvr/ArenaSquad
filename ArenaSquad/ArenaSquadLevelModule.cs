using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

// ReSharper disable UnusedType.Global

namespace ArenaSquad
{
    public class ArenaSquadLevelModule : LevelModule
    {
        public Color uniformColor;
        public List<SquadMember> members;
        private Player _player;
        private ArenaSquadData _arenaSquadData;
        private bool _isEnabled;

        public override IEnumerator OnLoadCoroutine(Level level)
        {
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            EventManager.onCreatureSpawn += EventManagerOnonCreatureSpawn;

            return base.OnLoadCoroutine(level);
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _arenaSquadData = GameManager.local.gameObject.GetComponent<ArenaSquadData>();
            _arenaSquadData.members = members;
        }
        
        private void EventManagerOnonCreatureSpawn(Creature creature)
        {
            if (creature.isPlayer)
            {
                _arenaSquadData.SpawnMembers(creature, uniformColor);
            }
        }
    }
}