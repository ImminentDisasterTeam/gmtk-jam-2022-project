using System;

namespace _Game.Scripts.GamePlay {
    public class DungeonRunner {
        private readonly Dungeon _dungeon;
        private readonly Action _onDungeonFinished;
        private readonly Room _room;

        public DungeonRunner(Dungeon dungeon, Action onDungeonFinished) {
            _dungeon = dungeon;
            _onDungeonFinished = onDungeonFinished;

            _room = dungeon.StartNextRoom();
        }

        private void OnContinue() {
            
        }
    }
}