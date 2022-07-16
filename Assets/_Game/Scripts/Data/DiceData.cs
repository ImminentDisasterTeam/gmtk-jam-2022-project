using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct DiceData {
        public string name;
        public string image;
        public int[] faces;
        public int minLevel;
        public int maxLevel;
    }
}