using UnityEngine;

namespace ArenaSquad
{
    public class ArenaSquadJSONData
    {
        public bool isEnabled { get; set; }
    }

    public class ArenaSquadData : MonoBehaviour
    {
        public ArenaSquadJSONData data;
    }
}