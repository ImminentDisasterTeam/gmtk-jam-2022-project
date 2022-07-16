using System;
using UnityEngine;

namespace _Game.Scripts.Data {
    [Serializable]
    public struct EnemyActionData {
        public string name;
        public SubActionData[] subActions;
        public ConditionData condition;
    }

    [Serializable]
    public struct SubActionData {
        [SerializeField] private string type;
        public EActionType Type => (EActionType) Enum.Parse(typeof(EActionType), type);
        public StatsData stats;
    }

    [Serializable]
    public struct ConditionData {
        public int chargeHigher;
        public int chargeLower;
        public int healthHigher;
        public int healthLower;
        public string elseAction;
    }
}