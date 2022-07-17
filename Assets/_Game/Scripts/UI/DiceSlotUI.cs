using System;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.UI {
    public class DiceSlotUI : SlotUI {
        [SerializeField] private Image _diceImage;
        [SerializeField] private GameObject _diceRoll;
        [SerializeField] private TextMeshProUGUI _diceRollText;

        public override EType Type => EType.Dice;

        protected override void PerformSwapWith(SlotUI other) {
            var otherSlot = (DiceSlotUI) other;
            (otherSlot.Dice, Dice) = (Dice, otherSlot.Dice);
        }

        protected override void LoadTooltip(Tooltip tooltip) {
            var diceTooltip = (DiceTooltip) tooltip;
            diceTooltip.Load(Dice);
        }

        protected override bool IsEmpty() => Dice == null;

        private readonly Action<DiceSlotUI, Dice, Dice> _onContentsChanged;
        public Event<DiceSlotUI, Dice, Dice> OnContentsChanged { get; }

        [CanBeNull] private Dice _dice;
        [CanBeNull]
        public Dice Dice {
            get => _dice;
            set {
                var oldValue = _dice;
                _dice = value;

                _diceImage.gameObject.SetActive(false);
                _diceRoll.SetActive(false);

                if (!(_dice is { } dice)) {
                    _onContentsChanged(this, oldValue, null);
                    return;
                }

                _diceImage.sprite = SpriteHolder.Instance.GetSprite(dice.Data.image);
                _diceImage.gameObject.SetActive(true);

                _onContentsChanged(this, oldValue, value);
            }
        }

        public DiceSlotUI() {
            OnContentsChanged = new Event<DiceSlotUI, Dice, Dice>(out _onContentsChanged);
        }

        public int Roll(Rng rng) {
            if (!(_dice is { } dice)) {
                throw new NullReferenceException("Dice slot is empty and cannot be rolled!");
            }

            var result = rng.NextChoice(dice.Data.faces);

            _diceRollText.text = result.ToString();
            _diceRoll.SetActive(true);

            return result;
        }
    }
}