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
        public ArenaSquadJSONData data;
    }
}