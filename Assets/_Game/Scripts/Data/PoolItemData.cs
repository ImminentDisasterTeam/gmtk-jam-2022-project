using System;
using UnityEngine;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct PoolItemData {
        public SubPoolItemData[] subPool;
    }

    [Serializable]
    public struct SubPoolItemData {
        public string type;
        [SerializeField] private int weight; // default - 1. Sets to 1 if zero

        public int Weight => weight == 0 ? 1 : weight;
    }
}