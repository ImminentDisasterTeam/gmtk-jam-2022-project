using System;
using UnityEngine;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.UI {
    public class ContinuePanelUI : UIElement {
        [SerializeField] private GameButton _continueButton;

        private readonly Action _onContinue;
        public Event OnContinue { get; }

        public ContinuePanelUI() {
            OnContinue = new Event(out _onContinue);
        }

        private void Awake() {
            _continueButton.OnClick.Subscribe(() => _onContinue());
        }

        public void SetEnabled(bool enabled) {
            _continueButton.SetEnabled(enabled);
        }
    }
}