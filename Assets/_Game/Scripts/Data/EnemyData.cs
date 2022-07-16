using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct EnemyData {
        public string name;
        public string image;
        public int health;
        public string[] actionLoop;
    }
}