using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.UI;
using GeneralUtils;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.GamePlay {
    public class Dungeon {
        private readonly DungeonData _data;
        private int _currentRoom = -1;
        public Sprite[] SpriteSheet => SpriteHolder.Instance.GetSheet(_data.spriteSheet);

        public Dungeon(DungeonData data) {
            _data = data;
        }

        [CanBeNull]
        public Room StartNextRoom(Rng rng) {
            if (_currentRoom + 1 >= _data.contentPool.Length) {
                return null;
            }

            _currentRoom++;

            var room = rng.NextWeightedChoice(_data.contentPool[_currentRoom].WeightedItems);
            var roomData = DataHolder.Instance.GetRooms().First(r => r.name == room);
            return new Room(roomData);
        }
    }
}