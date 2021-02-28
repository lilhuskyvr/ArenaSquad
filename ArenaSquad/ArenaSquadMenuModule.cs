using System.Collections;
using System.IO;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;

namespace ArenaSquad
{
    public class ArenaSquadMenuModule : MenuModule
    {
        private Button _statusButton;
        public static string modFolderName = "ArenaSquadU9";
        public ArenaSquadData arenaSquadData;

        public override void Init(MenuData menuData, Menu menu)
        {
            base.Init(menuData, menu);

            _statusButton = menu.GetCustomReference("StatusButton").GetComponent<Button>();
            
            LoadData();

            _statusButton.onClick.AddListener(() =>
            {
                arenaSquadData.data.isEnabled = !arenaSquadData.data.isEnabled;
                _statusButton.GetComponentInChildren<Text>().text =
                    arenaSquadData.data.isEnabled ? "Enabled" : "Disabled";
                SaveData();
            });
        }

        public void LoadData()
        {
            var filePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + "\\Data\\ArenaSquadData.json");

            var input = File.ReadAllText(filePath);

            var savedData =
                JsonConvert.DeserializeObject<ArenaSquadJSONData>(input);
            
            Debug.Log(input);
            Debug.Log("Loaded data successfully for menu module");

            arenaSquadData = GameManager.local.gameObject.AddComponent<ArenaSquadData>();
            arenaSquadData.data = savedData;
            _statusButton.GetComponentInChildren<Text>().text =
                arenaSquadData.data.isEnabled ? "Enabled" : "Disabled";
        }

        public void SaveData()
        {
            var filePath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + "\\Data\\ArenaSquadData.json");


            var json = JsonConvert.SerializeObject(arenaSquadData.data);

            File.WriteAllText(filePath, json);
        }
    }
}