using System;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = UnityEngine.Event;

namespace _Game.Scripts.UI {
    public class DiceActionPanelUI : UIElement {
        [SerializeField] private Image _actionIcon;
        [SerializeField] private TextMeshProUGUI _statsText;
        [SerializeField] private Transform _diceSlotsParent;
        [SerializeField] private DiceSlotUI _diceSlotPrefab;

        private bool _rolled;

        public bool CanRoll => GetDiceSlots().All(slot => slot.Dice != null) && !_rolled;
        public bool Empty => GetDiceSlots().All(slot => slot.Dice == null);

        private readonly Action<DiceActionPanelUI> _onContentsChanged;
        public Event<DiceActionPanelUI> OnContentsChanged { get; }

        private StatsData _stats;

        public DiceActionPanelUI() {
            OnContentsChanged = new Event<DiceActionPanelUI>(out _onContentsChanged);
        }

        public void Load(EActionType actionType, StatsData stats) {
            var shouldShowStats = actionType != EActionType.Wait;
            var diceCount = shouldShowStats ? stats.dices : 0;

            _actionIcon.sprite = SpriteHolder.Instance.GetSprite(actionType);

            _rolled = false;
            _stats = stats;
            _statsText.text = stats.ToString();
            _statsText.gameObject.SetActive(shouldShowStats);
            for (var i = 0; i < diceCount; i++) {
                var diceSlot = Instantiate(_diceSlotPrefab, _diceSlotsParent);
                diceSlot.State = SlotUI.EState.CanPut;
                diceSlot.OnContentsChanged.Subscribe(OnDiceSlotChanged);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
        }

        private void OnDiceSlotChanged(DiceSlotUI slot, Dice oldValue, Dice value) {
            if (oldValue == null && value != null) {
                slot.State = SlotUI.EState.Locked;
                _onContentsChanged(this);
            }
        }

        public override void Hide(Action onDone = null) {
            base.Hide(() => {
                foreach (var slot in GetDiceSlots()) {
                    slot.OnContentsChanged.Unsubscribe(OnDiceSlotChanged);
                    Destroy(slot.gameObject);
                }
                onDone?.Invoke();
            });
        }

        public int Roll(Rng rng) {
            _rolled = true;
            return _stats.initial + Convert.ToInt32(Math.Floor(_stats.dicesMod * GetDiceSlots().Select(s => s.Roll(rng)).Sum()));
        }

        private DiceSlotUI[] GetDiceSlots() {
            return Enumerable
                .Range(0, _diceSlotsParent.childCount)
                .Select(_diceSlotsParent.GetChild)
                .Select(t => t.GetComponent<DiceSlotUI>())
                .ToArray();
        }
    }
}