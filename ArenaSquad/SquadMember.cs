using System;
using System.Collections;
using ThunderRoad;
using UnityEngine;

namespace ArenaSquad
{
    public class SquadMember
    {
        public bool enabled { get; set; }
        public string creatureId { get; set; }
        public string containerId { get; set; }
        public string brainId { get; set; }
        public Creature creature { get; set; }

        private Color _uniformColor;

        private IEnumerator PaintCreature(Creature creatureForPainting, Color color)
        {
            creatureForPainting.Hide(true);
            yield return new WaitForSeconds(2);
            foreach (var part in creatureForPainting.manikinLocations.PartList.GetAllParts())
            {
                foreach (var renderer in part.GetRenderers())
                {
                    foreach (var material in renderer.materials)
                    {
                        var materialName = material.name.ToLower();

                        if (
                            !materialName.Contains("body")
                            && !materialName.Contains("eye")
                            && !materialName.Contains("hair")
                            && !materialName.Contains("head")
                            && !materialName.Contains("male_hands")
                            && !materialName.Contains("mouth")
                        )

                            if (material.HasProperty("_BaseColor"))
                                material.SetColor("_BaseColor", color);
                    }
                }
            }

            creatureForPainting.Hide(false);
            yield return null;
        }

        private Vector3 FindSpawningLocation(Creature player)
        {
            var creatureData = Catalog.GetData<CreatureData>(creatureId);
            var playerTransform = player.transform;
            var colliders = Physics.OverlapSphere(playerTransform.position, 5);
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    if (i != 0 && j != 0)
                    {
                        var found = false;
                        var creaturePosition = playerTransform.position + i * playerTransform.forward +
                                               j * playerTransform.right;
                        foreach (var collider in colliders)
                        {
                            if (Vector3.Distance(creaturePosition, collider.transform.position) <
                                creatureData.randomMinHeight / 4)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            return creaturePosition;
                        }
                    }
                }
            }

            return Vector3.zero;
        }

        public void SpawnCreature(Creature player, Color uniformColor)
        {
            _uniformColor = uniformColor;
            var spawningLocation = FindSpawningLocation(player);

            if (spawningLocation != Vector3.zero)
            {
                var creatureData = Catalog.GetData<CreatureData>(creatureId);
                creatureData.brainId = brainId;
                creatureData.containerID = containerId;
                GameManager.local.StartCoroutine(creatureData.SpawnCoroutine(
                    spawningLocation,
                    player.transform.rotation,
                    null,
                    squadMember =>
                    {
                        squadMember.SetFaction(2);
                        var brainHuman = squadMember.brain.instance as BrainHuman;
                        brainHuman.canLeave = false;
                        brainHuman.allowDisarm = false;
                        brainHuman.allowRearm = false;
                        brainHuman.allowShieldRearm = false;
                        creature = squadMember;
                        squadMember.OnKillEvent += SquadMemberOnOnKillEvent;
                        GameManager.local.StartCoroutine(PaintCreature(squadMember, uniformColor));
                    }));
            }
        }

        private void SquadMemberOnOnKillEvent(CollisionInstance collisioninstance, EventTime eventtime)
        {
            var arenaSquadData = GameManager.local.GetComponent<ArenaSquadData>();

            if (arenaSquadData.data.isEnabled)
            {
                SpawnCreature(Player.local.creature, _uniformColor);
            }
        }
    }
}