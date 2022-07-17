using System;
using _Game.Scripts.Data;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.GamePlay {
    public class DungeonRunner {
        private readonly Dungeon _dungeon;
        private readonly Rng _rng;
        private readonly Action<RoomData, Rng, Sprite, Action<bool>> _roomStarter;
        private readonly Action<bool> _onDungeonFinished;

        private Room _room;

        public DungeonRunner(Dungeon dungeon, Rng rng, Action<RoomData, Rng, Sprite, Action<bool>> roomStarter, Action<bool> onDungeonFinished) {
            _dungeon = dungeon;
            _rng = rng;
            _roomStarter = roomStarter;
            _onDungeonFinished = onDungeonFinished;
            
            StartNextRoom();
        }

        private void StartNextRoom() {
            var room = _dungeon.StartNextRoom(_rng);
            if (room == null) {
                _onDungeonFinished(false);
                return;
            }

            var background = _rng.NextChoice(_dungeon.SpriteSheet);
            _roomStarter(room.Data, _rng, background, OnRoomFinished);
        }

        private void OnRoomFinished(bool finishedByDeath) {
            if (finishedByDeath) {
                _onDungeonFinished(true);
                return;
            }

            StartNextRoom();
        }
    }
}