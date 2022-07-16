using System;
using _Game.Scripts.Data;

namespace _Game.Scripts.GamePlay {
    public class Room {
        public RoomData Data { get; }

        public Room(RoomData data) {
            Data = data;
        }
    }
}