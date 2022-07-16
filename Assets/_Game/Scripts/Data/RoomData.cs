using System;
using UnityEngine;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct RoomData {
        public string name;
        public string image;
        [SerializeField] private string type;
        public ERoomType Type => (ERoomType) Enum.Parse(typeof(ERoomType), type);
        public string text;
        // optional for different types
        public PoolItemData[] contentPool;
        public int[] check;
        public int[] healthChange;
        public bool mandatory;
        public int gold;
        public int hiddenCheck;
        public int hiddenGold;
        public PoolItemData[] hiddenContentPool;
    }
}