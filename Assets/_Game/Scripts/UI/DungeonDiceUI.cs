using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class DungeonDiceUI : UIElement {
        [SerializeField] private Transform _diceSlotsParent;
        [SerializeField] private DiceSlotUI _diceSlotPrefab;
        [SerializeField] private TextMeshProUGUI _discardLabel;
        [SerializeField] private TextMeshProUGUI _deckLabel;

        private readonly List<Dice> _deckList = new List<Dice>();
        private readonly List<Dice> _discardList = new List<Dice>();
        private Rng _rng;

        public static DungeonDiceUI Instance { get; private set; }

        public DungeonDiceUI() {
            Instance = this;
        }

        public void Load(IEnumerable<Dice> dices, int diceSlots, Rng rng) {
            _rng = rng;

            _deckList.Clear();
            _deckList.AddRange(dices.Where(dice => dice != null));
            _discardList.Clear();
            UpdateLabels();

            for (var i = 0; i < diceSlots; i++) {
                var diceSlot = Instantiate(_diceSlotPrefab, _diceSlotsParent);
                diceSlot.OnContentsChanged.Subscribe(OnDiceSlotChanged);
                // diceSlot.State = SlotUI.EState.CanTake;
            }

            Draw();
        }

        private void OnDiceSlotChanged(DiceSlotUI slot, Dice oldValue, Dice value) {
            if (oldValue != null && value == null) {
                slot.State = SlotUI.EState.Locked;
                // _discardList.Add(oldValue);
                // UpdateLabels();
            }

            if (GetDiceSlots().All(s => s.Dice == null)) {
                Draw();
            }
        }

        public void AddToDiscard(IEnumerable<Dice> dices) {
            _discardList.AddRange(dices);
            UpdateLabels();
        }

        private void Draw() {
            var diceSlots = GetDiceSlots();
            foreach (var diceSlot in diceSlots) {
                if (_deckList.Count == 0) {
                    MoveDiscardToDeck();
                }

                diceSlot.OnContentsChanged.Unsubscribe(OnDiceSlotChanged);

                if (_deckList.Count != 0) {
                    var dice = _rng.NextChoice(_deckList);
                    _deckList.Remove(dice);
                    
                    diceSlot.State = SlotUI.EState.CanTake;
                    diceSlot.Dice = dice;
                } else {
                    diceSlot.State = SlotUI.EState.Locked;
                    diceSlot.Dice = null;
                }

                diceSlot.OnContentsChanged.Subscribe(OnDiceSlotChanged);
            }

            UpdateLabels();
        }

        private void MoveDiscardToDeck() {
            _deckList.AddRange(_discardList);
            _discardList.Clear();
        }

        private void UpdateLabels() {
            _deckLabel.text = _deckList.Count.ToString();
            _discardLabel.text = _discardList.Count.ToString();
        }

        public override void Hide(Action onDone = null) {
            base.Hide(() => {
                foreach (var slot in GetDiceSlots()) {
                    slot.OnContentsChanged.Unsubscribe(OnDiceSlotChanged);
                    DestroyImmediate(slot.gameObject);
                }
            });
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