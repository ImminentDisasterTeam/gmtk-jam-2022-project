using System.Linq;
using _Game.Scripts.Data;
using GeneralUtils;
using JetBrains.Annotations;

namespace _Game.Scripts.GamePlay {
    public class Dungeon {
        private readonly DungeonData _data;
        private int _currentRoom = -1;

        public Dungeon(DungeonData data) {
            _data = data;
        }

        [CanBeNull]
        public Room StartNextRoom(Rng rng) {
            if (_currentRoom + 1 >= _data.contentPool.Length) {
                return null;
            }

            _currentRoom++;

            var possibleRooms = _data.contentPool[_currentRoom].subPool;
            var room = rng.NextWeightedChoice(possibleRooms
                .Select(r => (r.type, (float) r.Weight))
                .ToList());

            var roomData = DataHolder.Instance.GetRooms().First(r => r.name == room);
            return new Room(roomData);
        }
    }
}