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
        private Dictionary<string, int> _maxAlives = new Dictionary<string, int>();

        public override IEnumerator OnLoadCoroutine(Level level)
        {
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            EventManager.onCreatureSpawn += EventManagerOnonCreatureSpawn;
            EventManager.onCreatureKill += EventManagerOnonCreatureKill;

            for (int i = 0; i < Catalog.data.Length; i++)
            {
                foreach (CatalogData catalogData in Catalog.data[i])
                {
                    if (catalogData is WaveData)
                    {
                        _maxAlives[catalogData.id] = (catalogData as WaveData).maxAlive;
                    }
                }
            }

            return base.OnLoadCoroutine(level);
        }

        private void EventManagerOnonCreatureKill(Creature creature, Player player, CollisionInstance collisioninstance,
            EventTime eventtime)
        {
            if (eventtime == EventTime.OnEnd)
            {
                if (_arenaSquadData.data.isEnabled)
                {
                    foreach (var member in members)
                    {
                        if (ReferenceEquals(member.squadMemberCreature, creature))
                        {
                            member.SpawnCreature(Player.local.creature, uniformColor);
                        }
                    }
                }
            }
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _arenaSquadData = GameManager.local.gameObject.GetComponent<ArenaSquadData>();
            _arenaSquadData.members = members;
            _arenaSquadData.uniformColor = uniformColor;
            _arenaSquadData.maxAlives = _maxAlives;
            
            _arenaSquadData.OnDataChanged();
        }

        private void EventManagerOnonCreatureSpawn(Creature creature)
        {
            if (creature.isPlayer)
            {
                _arenaSquadData.SpawnMembers(creature);
            }
        }
    }
}