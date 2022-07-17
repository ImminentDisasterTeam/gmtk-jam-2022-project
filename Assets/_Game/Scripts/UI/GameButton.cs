using System;
using UnityEngine;
using UnityEngine.UI;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.UI {
    public class GameButton : MonoBehaviour {
        [SerializeField] private Button _button;

        private readonly Action _onClick;
        public Event OnClick { get; }

        public GameButton() {
            OnClick = new Event(out _onClick);
        }

        public void SetEnabled(bool enabled) {
            _button.interactable = enabled;
        }

        private void Awake() {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick() {
            // TODO sound
            _onClick();
        }
    }
}