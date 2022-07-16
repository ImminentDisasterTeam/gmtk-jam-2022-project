using System;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct StatsData {
        public int initial;
        public int dices;
        public float dicesMod;

        public override string ToString() {
            return $"{initial}" + (dices != 0 && dicesMod != 0 ? $" + {dicesMod} x" : "");
        }
    }
}