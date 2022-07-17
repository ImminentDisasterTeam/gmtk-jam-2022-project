using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InfoPanelUI : UIElement {
        [SerializeField] private TextMeshProUGUI _text;

        public static InfoPanelUI Instance { get; private set; }

        public InfoPanelUI() {
            Instance = this;
        }

        public void LoadEscape(int required) {
            _text.text = $"Get {required} or more to successfully escape.";
        }

        public void LoadEscapeResult(bool success) {
            _text.text = $"You escape was {(success ? "successful" : "a big failure")}.";
        }
    }
}