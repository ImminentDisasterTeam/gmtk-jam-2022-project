using System;
using _Game.Scripts.Data;

namespace _Game.Scripts.GamePlay {
    public class Room {
        private readonly RoomData _data;

        public Room(RoomData data) {
            _data = data;
        }

        public bool Continue() {
            return true;
        }
    }
}