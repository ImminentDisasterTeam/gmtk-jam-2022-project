using System;
using JetBrains.Annotations;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct PlayerSaveData {
        public int maxHealth;
        public int money;
        public string image;
        public string attackSlot;
        public string defenceSlot;
        public string interactSlot;
        public StatsData defaultAttack;
        public StatsData defaultDefence;
        public StatsData defaultInteraction;
        public string[] dices;
        public string[] inventory;
    }
}