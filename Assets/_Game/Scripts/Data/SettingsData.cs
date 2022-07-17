using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct SettingsData {
        public int handSize;
        public int deckSize;
        public int inventorySize;
        public string[] shopInventory;
        public float sellMultiplier;
        public float goldRetainMultiplier;
        public int maxLevel;
        public string[] initialDices;
    }
}