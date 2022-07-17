using System;
using UnityEngine;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct StatsData {
        public int initial;
        [SerializeField] private int dices;
        public int DiceCount => dices == 0 || dicesMod == 0 ? 0 : dices;
        public float dicesMod;

        public override string ToString() {
            return $"{initial}" + (dices != 0 && dicesMod != 0 ? $" + {dicesMod} x" : "");
        }
    }
}