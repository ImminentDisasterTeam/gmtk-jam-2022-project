using System;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using PlasticGui;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class MainMenuUI : UIElement {
        [SerializeField] private GameButton _start;
        [SerializeField] private GameButton _reset;
        [SerializeField] private MainGameMenuUI _mainGameMenu;

        private Rng _rng;

        private void Awake() {
            _start.OnClick.Subscribe(OnStart);
            _reset.OnClick.Subscribe(OnReset);
        }

        public void Load(Rng rng) {
            _rng = rng;
        }

        private void OnStart() {
            Hide(() => {
                _mainGameMenu.Load(_rng);
                _mainGameMenu.Show();
            });
        }

        private void OnReset() {
            Player.Instance.Reset(_rng);
        }
    }
}