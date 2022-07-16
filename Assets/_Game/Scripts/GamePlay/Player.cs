using System;
using _Game.Scripts.Data;

namespace _Game.Scripts.GamePlay {
    public class Player {
        public static Player Instance { get; private set; }

        public StatsData AttackStats { get; private set; }
        public StatsData DefenceStats { get; private set; }
        public StatsData InteractionStats { get; private set; }

        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        public int Money { get; private set; }

        public static void LoadPlayer() {
            Instance = new Player();
            // TODO load
        }

        public void ChangeMoney(int deltaMoney) {
            Money += deltaMoney;
            // TODO writeToSave
        }

        public bool ChangeHealth(int deltaHealth) {
            Health = Math.Max(0, Health + deltaHealth);
            // TODO writeToSave
            return Health == 0;
        }
    }
}