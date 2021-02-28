using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ThunderRoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArenaSquad
{
    public class ArenaSquadListBuilderLevelModule : LevelModule
    {
        public static string modFolderName = "ArenaSquadU9";
        public override IEnumerator OnLoadCoroutine(Level level)
        {
            var containerIds = new List<string>();
            var creatureIds = new List<string>();
            var brainIds = new List<string>();
            for (int i = 0; i < Catalog.data.Length; i++)
            {
                foreach (CatalogData catalogData in Catalog.data[i])
                {
                    if (catalogData is BrainData)
                    {
                        brainIds.Add(catalogData.id);
                    }

                    if (catalogData is ContainerData)
                    {
                        containerIds.Add(catalogData.id);
                    }

                    if (catalogData is CreatureData)
                    {
                        creatureIds.Add(catalogData.id);
                    }
                }
            }
            
            var containerIdsPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + "\\Data\\ContainerIds.txt");
            var creatureIdsPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + "\\Data\\CreatureIds.txt");
            var brainIdsPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + "\\Data\\BrainIds.txt");

            File.WriteAllText(brainIdsPath, string.Join(Environment.NewLine, brainIds));
            File.WriteAllText(containerIdsPath, string.Join(Environment.NewLine, containerIds));
            File.WriteAllText(creatureIdsPath, string.Join(Environment.NewLine, creatureIds));
            
            return base.OnLoadCoroutine(level);
        }
    }
}