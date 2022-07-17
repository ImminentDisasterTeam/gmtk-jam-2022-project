using System;
using _Game.Scripts.GamePlay;
using _Game.Scripts.UI;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts {
    public class App : MonoBehaviour {
        [SerializeField] private MainGameMenuUI _mainGameMenu;

        private void Start() {
            Debug.Log("Start");

            var stamp = (int) new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var rng = new Rng(stamp);
            Player.LoadPlayer(rng);

            _mainGameMenu.Load(rng);
            _mainGameMenu.Show();
            SoundHolder.Instance.PlayMusic("main");
        }
    }
}