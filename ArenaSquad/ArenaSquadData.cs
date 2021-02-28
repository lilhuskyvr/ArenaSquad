using System;
using System.Collections;
using System.Collections.Generic;
using ThunderRoad;
using UnityEngine;

namespace ArenaSquad
{
    // ReSharper disable once InconsistentNaming
    public class ArenaSquadJSONData
    {
        // ReSharper disable once InconsistentNaming
        public bool isEnabled { get; set; }
    }

    public class ArenaSquadData : MonoBehaviour
    {
        public List<SquadMember> members;
        public ArenaSquadJSONData data;
        public Color uniformColor;

        private void Start()
        {
            members = new List<SquadMember>();
        }

        public void OnDataChanged()
        {
            var memberCount = GetMemberCount();

            if (data.isEnabled)
            {
                //if enabled
                for (int i = 0; i < Catalog.data.Length; i++)
                {
                    foreach (CatalogData catalogData in Catalog.data[i])
                    {
                        if (catalogData is WaveData)
                        {
                            (catalogData as WaveData).maxAlive += memberCount;
                        }
                    }
                }
            }
            else
            {
                //if disabled
                foreach (var member in members)
                {
                    if (member.squadMemberCreature != null)
                    {
                        member.squadMemberCreature.Kill();
                        member.squadMemberCreature = null;
                    }
                }

                for (int i = 0; i < Catalog.data.Length; i++)
                {
                    foreach (CatalogData catalogData in Catalog.data[i])
                    {
                        if (catalogData is WaveData)
                        {
                            (catalogData as WaveData).maxAlive -= memberCount;
                        }
                    }
                }
            }
        }
        public int GetMemberCount()
        {
            var memberCount = 0;
            foreach (var member in members)
            {
                if (member.enabled)
                    memberCount++;
            }

            return memberCount;
        }

        public void SpawnMembers(Creature player)
        {
            if (data.isEnabled)
            {
                foreach (var member in members)
                {
                    if (member.enabled)
                    {
                        member.SpawnCreature(player, uniformColor);
                    }
                }
            }
        }
    }
}