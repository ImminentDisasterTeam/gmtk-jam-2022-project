using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct DiceData {
        public string name;
        public string displayName;
        public string desc;
        public string image;
        public int[] faces;
        public int minLevel;
        public int maxLevel;
    }
}