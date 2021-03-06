using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct SettingsData {
        public int handSize;
        public int deckSize;
        public int inventorySize;
        public int dicesPerLevel;
        public string[] shopInventory;
        public float sellMultiplier;
        public float goldRetainMultiplier;
        public int maxLevel;
        public string[] initialDices;
        public int initialEscapeThreshold;
        public int escapeThresholdStep;
    }
}