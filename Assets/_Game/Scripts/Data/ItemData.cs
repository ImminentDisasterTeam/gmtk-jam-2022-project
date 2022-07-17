using System;
using UnityEngine;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct ItemData {
        public string name;
        public string displayName;
        public string desc;
        [SerializeField] private string type;
        public EItemType Type => (EItemType) Enum.Parse(typeof(EItemType), type);
        public string image;
        public StatsData stats;
        public int cost;
    }
}