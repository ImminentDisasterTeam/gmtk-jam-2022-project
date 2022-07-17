using System;
using System.Linq;
using _Game.Scripts.Data;
using GeneralUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class EnemyUI : UIElement {
        [SerializeField] private Transform _actionPanelsParent;
        [SerializeField] private DiceActionPanelUI _actionPanelPrefab;
        [SerializeField] private TextMeshProUGUI _health;
        [SerializeField] private TextMeshProUGUI _charge;
        [SerializeField] private Image _image;

        private int _maxHealth;
        private ValueWaiter<int> _allPanelsReadyWaiter;

        public void Load(int maxHealth, Sprite sprite) {
            _maxHealth = maxHealth;
            SetHealth(maxHealth);
            SetCharge(0);
            _image.sprite = sprite;
        }

        public void SetAction(SubActionData[] actions, Action onReady) {
            ClearSlots();

            _allPanelsReadyWaiter = new ValueWaiter<int>(actions.Length);
            _allPanelsReadyWaiter.WaitFor(0, onReady);

            foreach (var action in actions) {
                var actionPanel = Instantiate(_actionPanelPrefab, _actionPanelsParent);
                actionPanel.Load(action.Type, action.stats);
                actionPanel.OnContentsChanged.Subscribe(OnPanelChanged);
                OnPanelChanged(actionPanel);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) _actionPanelsParent);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
        }

        public int[] Roll(Rng rng) {
            return GetActionPanels().Select(p => p.Roll(rng)).ToArray();
        }

        private void OnPanelChanged(DiceActionPanelUI panel) {
            if (panel.CanRoll) {
                _allPanelsReadyWaiter.Value--;
            }
        }

        public void SetHealth(int health) {
            _health.text = $"{health} / {_maxHealth}";
        }

        public void SetCharge(int charge) {
            _charge.text = charge.ToString();
        }

        public override void Hide(Action onDone = null) {
            base.Hide(() => {
                ClearSlots();
                onDone?.Invoke();
            });
        }

        private void ClearSlots() {
            foreach (var panel in GetActionPanels()) {
                panel.OnContentsChanged.Unsubscribe(OnPanelChanged);
                DestroyImmediate(panel.gameObject);
            }
        }

        protected DiceActionPanelUI[] GetActionPanels() {
            return Enumerable
                .Range(0, _actionPanelsParent.childCount)
                .Select(_actionPanelsParent.GetChild)
                .Select(t => t.GetComponent<DiceActionPanelUI>())
                .Where(c => c != null)
                .ToArray();
        }
    }
}