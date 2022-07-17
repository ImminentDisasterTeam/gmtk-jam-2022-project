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

        public DataHolder() {
            Instance = this;
        }

        private DiceData[] _dicesCache;
        public DiceData[] GetDices() {
            return _dicesCache ??= JsonUtility.FromJson<Dices>(_dices.text).dices;
        }

        private DungeonData[] _dungeonsCache;
        public DungeonData[] GetDungeons() {
            return _dungeonsCache ??= JsonUtility.FromJson<Dungeons>(_dungeons.text).dungeons;
        }

        private EnemyData[] _enemiesCache;
        public EnemyData[] GetEnemies() {
            return _enemiesCache ??= JsonUtility.FromJson<Enemies>(_enemies.text).enemies;
        }

        private EnemyActionData[] _enemyActionsCache;
        public EnemyActionData[] GetEnemyActions() {
            return _enemyActionsCache ??= JsonUtility.FromJson<EnemyActions>(_enemyActions.text).actions;
        }

        private ItemData[] _itemsCache;
        public ItemData[] GetItems() {
            return _itemsCache ??= JsonUtility.FromJson<Items>(_items.text).items;
        }

        private PlayerData[] _playersCache;
        public PlayerData[] GetPlayers() {
            return _playersCache ??= JsonUtility.FromJson<Players>(_players.text).players;
        }

        private RoomData[] _roomsCache;
        public RoomData[] GetRooms() {
            return _roomsCache ??= JsonUtility.FromJson<Rooms>(_rooms.text).rooms;
        }

        private SettingsData? _settingsDataCache;
        public SettingsData GetSettings() {
            return _settingsDataCache ??= JsonUtility.FromJson<SettingsData>(_settings.text);
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