using _Game.Scripts.Data;
using UnityEngine;

namespace _Game.Scripts {
    public class App : MonoBehaviour {
        private void Start() {
            var dices = DataHolder.Instance.GetDices();
            var dungeons = DataHolder.Instance.GetDungeons();
            var enemies = DataHolder.Instance.GetEnemies();
            var enemyActions = DataHolder.Instance.GetEnemyActions();
            var items = DataHolder.Instance.GetItems();
            var players = DataHolder.Instance.GetPlayers();
            var rooms = DataHolder.Instance.GetRooms();
            var settings = DataHolder.Instance.GetSettings();
            Debug.Log("Start");
        }
    }
}