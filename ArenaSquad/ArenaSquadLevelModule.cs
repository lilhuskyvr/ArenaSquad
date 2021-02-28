using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArenaSquad
{
    public class ArenaSquadLevelModule : LevelModule
    {
        public List<SquadMember> members;
        public Color uniformColor;
        private Player _player;
        private int _memberCount;
        private ArenaSquadData _arenaSquadData;
        private bool _isEnabled;

        public override IEnumerator OnLoadCoroutine(Level level)
        {
            EventManager.onPlayerSpawn += EventManagerOnonPlayerSpawn;
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
            _memberCount = 0;
            foreach (var member in members)
            {
                if (member.enabled)
                    _memberCount++;
            }

            return base.OnLoadCoroutine(level);
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            foreach (var member in members)
            {
                member.creature = null;
            }
        }

        private void EventManagerOnonPlayerSpawn(Player player)
        {
            _player = Player.local;
        }

        public override void Update(Level level)
        {
            if (_player == null)
                return;

            try
            {
                if (_arenaSquadData == null)
                    _arenaSquadData = GameManager.local.gameObject.GetComponent<ArenaSquadData>();

                if (_arenaSquadData.data.isEnabled)
                {
                    foreach (var member in members)
                    {
                        if (member.enabled)
                        {
                            if (member.creature == null || member.creature.state == Creature.State.Dead)
                            {
                                member.SpawnCreature(_player, uniformColor);
                            }
                        }
                    }
                }

                if (!_arenaSquadData.data.isEnabled && _isEnabled)
                {
                    
                    //clean
                    foreach (var member in members)
                    {
                        if (member.creature != null)
                        {
                            member.creature.Despawn();
                            member.creature = null;
                        }
                    }
                    for (int i = 0; i < Catalog.data.Length; i++)
                    {
                        foreach (CatalogData catalogData in Catalog.data[i])
                        {
                            if (catalogData is WaveData)
                            {
                                (catalogData as WaveData).maxAlive -= _memberCount;
                            }
                        }
                    }
                }

                if (_arenaSquadData.data.isEnabled && !_isEnabled)
                {
                    for (int i = 0; i < Catalog.data.Length; i++)
                    {
                        foreach (CatalogData catalogData in Catalog.data[i])
                        {
                            if (catalogData is WaveData)
                            {
                                (catalogData as WaveData).maxAlive += _memberCount;
                            }
                        }
                    }
                }
                
                _isEnabled = _arenaSquadData.data.isEnabled;

            }
            #pragma warning disable 168
            catch (Exception exception)
            #pragma warning restore 168
            {
                // ignored
            }
        }
    }
}