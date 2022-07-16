using System;
using UnityEngine;
using UnityEngine.UI;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.UI {
    public class ContinuePanelUI : UIElement {
        [SerializeField] private Button _continueButton;

        private readonly Action _onContinue;
        public Event OnContinue { get; }

        public ContinuePanelUI() {
            OnContinue = new Event(out _onContinue);
        }

        private void Awake() {
            _continueButton.onClick.AddListener(() => _onContinue());
        }

        public void SetEnabled(bool enabled) {
            _continueButton.enabled = enabled;
        }
    }
}