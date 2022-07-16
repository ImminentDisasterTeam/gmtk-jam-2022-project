using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Scripts.Data {
    public class DataHolder : MonoBehaviour {
        [SerializeField] private TextAsset _dices;
        [SerializeField] private TextAsset _dungeons;
        [SerializeField] private TextAsset _enemies;
        [SerializeField] private TextAsset _enemyActions;
        [SerializeField] private TextAsset _items;
        [SerializeField] private TextAsset _players;
        [SerializeField] private TextAsset _rooms;
        [SerializeField] private TextAsset _settings;

        public static DataHolder Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        public DiceData[] GetDices() {
            return JsonUtility.FromJson<Dices>(_dices.text).dices;
        }

        public DungeonData[] GetDungeons() {
            return JsonUtility.FromJson<Dungeons>(_dungeons.text).dungeons;
        }

        public EnemyData[] GetEnemies() {
            return JsonUtility.FromJson<Enemies>(_enemies.text).enemies;
        }

        public EnemyActionData[] GetEnemyActions() {
            return JsonUtility.FromJson<EnemyActions>(_enemyActions.text).actions;
        }

        public ItemData[] GetItems() {
            return JsonUtility.FromJson<Items>(_items.text).items;
        }

        public PlayerData[] GetPlayers() {
            return JsonUtility.FromJson<Players>(_players.text).players;
        }

        public RoomData[] GetRooms() {
            return JsonUtility.FromJson<Rooms>(_rooms.text).rooms;
        }

        public SettingsData GetSettings() {
            return JsonUtility.FromJson<SettingsData>(_settings.text);
        }

        [Serializable]
        private struct Dices {
            public DiceData[] dices;
        }

        [Serializable]
        private struct Dungeons {
            public DungeonData[] dungeons;
        }

        [Serializable]
        private struct Enemies {
            public EnemyData[] enemies;
        }

        [Serializable]
        private struct EnemyActions {
            public EnemyActionData[] actions;
        }

        [Serializable]
        private struct Items {
            public ItemData[] items;
        }

        [Serializable]
        private struct Players {
            public PlayerData[] players;
        }

        [Serializable]
        private struct Rooms {
            public RoomData[] rooms;
        }
    }
}