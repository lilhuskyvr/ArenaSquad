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

        private IEnumerator PaintCreature(Creature creature, Color color)
        {
            creature.Hide(true);
            yield return new WaitForSeconds(2);
            foreach (var part in creature.manikinLocations.PartList.GetAllParts())
            {
                foreach (var renderer in part.GetRenderers())
                {
                    foreach (var material in renderer.sharedMaterials)
                    {
                        var materialName = material.name.ToLower();

                        if (
                            !materialName.Contains("body")
                            && !materialName.Contains("eye")
                            && !materialName.Contains("hair")
                            && !materialName.Contains("head")
                            && !materialName.Contains("male_hands")
                        )

                            if (material.HasProperty("_BaseColor"))
                                material.SetColor("_BaseColor", color);
                    }
                }
            }

            creature.Hide(false);
            yield return null;
        }

        private Vector3 FindSpawningLocation(Player player)
        {
            var creatureData = Catalog.GetData<CreatureData>(creatureId);
            var colliders = Physics.OverlapSphere(player.transform.position, 5);
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    if (i != 0 && j != 0)
                    {
                        var found = false;
                        var creaturePosition = player.transform.position + i * player.transform.forward +
                                               j * player.transform.right;
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

        public void SpawnCreature(Player player, Color uniformColor)
        {
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
                        GameManager.local.StartCoroutine(PaintCreature(squadMember, uniformColor));
                    }));
            }
        }
    }
}