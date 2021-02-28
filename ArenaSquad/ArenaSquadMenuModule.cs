using System.IO;
using Newtonsoft.Json;
using ThunderRoad;
using UnityEngine.UI;

namespace ArenaSquad
{
    public class ArenaSquadMenuModule : MenuModule
    {
        private Button _statusButton;
        public string modFolderName = "ArenaSquadU9";
        public string dataFilePath = "\\Data\\ArenaSquadData.json";

        // ReSharper disable once InconsistentNaming
        public ArenaSquadData arenaSquadData;

        public override void Init(MenuData menuData, Menu menu)
        {
            base.Init(menuData, menu);

            _statusButton = menu.GetCustomReference("StatusButton").GetComponent<Button>();

            LoadData();
            
            _statusButton.GetComponentInChildren<Text>().text =
                arenaSquadData.data.isEnabled ? "Enabled" : "Disabled";

            _statusButton.onClick.AddListener(() =>
            {
                arenaSquadData.data.isEnabled = !arenaSquadData.data.isEnabled;
                _statusButton.GetComponentInChildren<Text>().text =
                    arenaSquadData.data.isEnabled ? "Enabled" : "Disabled";
                SaveData();
            });
        }

        private string GetDataFilePath()
        {
            return FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods,
                modFolderName + dataFilePath);
        }

        public void LoadData()
        {
            var jsonInput = File.ReadAllText(GetDataFilePath());

            var savedData =
                JsonConvert.DeserializeObject<ArenaSquadJSONData>(jsonInput, Catalog.GetJsonNetSerializerSettings());

            arenaSquadData = GameManager.local.gameObject.AddComponent<ArenaSquadData>();
            arenaSquadData.data = savedData;
        }

        public void SaveData()
        {
            var json = JsonConvert.SerializeObject(arenaSquadData.data, Catalog.GetJsonNetSerializerSettings());

            File.WriteAllText(GetDataFilePath(), json);
        }
    }
}