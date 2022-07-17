using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct PlayerData {
        public string name;
        public string image;
        public int maxHealth;
        public StatsData attack;
        public StatsData defence;
        public StatsData interaction;
    }
}