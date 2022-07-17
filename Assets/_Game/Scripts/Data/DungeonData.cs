using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct DungeonData {
        public int[] levels;
        public string spriteSheet;
        public PoolItemData[] contentPool;
    }
}