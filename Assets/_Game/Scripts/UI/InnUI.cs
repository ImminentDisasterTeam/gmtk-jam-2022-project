using System;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class InnUI : UIElement {
        [SerializeField] private Transform _choiceParent;
        [SerializeField] private Transform _deck;
        [SerializeField] private Transform _deckParent;
        [SerializeField] private DiceSlotUI _diceSlotPrefab;
        [SerializeField] private GameObject _info;
        [SerializeField] private GameButton _return;

        private Action _onReturn;

        public void Load(Rng rng, Action onReturn) {
            ClearItems();
            
            _onReturn = onReturn;
            _return.OnClick.Subscribe(OnReturnClick);
            var hasChoice = Player.Instance.HasDicesChoice;
            
            _choiceParent.gameObject.SetActive(hasChoice);
            _deck.gameObject.SetActive(hasChoice);
            _info.SetActive(!hasChoice);

            if (!hasChoice) {
                return;
            }

            foreach (var diceName in Player.Instance.ThisLevelDices) {
                var dice = new Dice(DataHolder.Instance.GetDices().First(d => d.name == diceName));

                var choiceSlot = Instantiate(_diceSlotPrefab, _choiceParent);
                choiceSlot.Dice = dice;
                choiceSlot.State = SlotUI.EState.CanTake;
            }

            foreach (var dice in Player.Instance.Dices) {
                var deckSlot = Instantiate(_diceSlotPrefab, _deckParent);
                deckSlot.Dice = dice;
                deckSlot.State = SlotUI.EState.CanPut;
                deckSlot.OnContentsChanged.Subscribe(OnDeckSlotChanged);
            }
        }

        private void OnDeckSlotChanged(DiceSlotUI slot, Dice oldValue, Dice value) {
            var diceIndex = Enumerable
                .Range(0, _deckParent.childCount)
                .Select(_deckParent.GetChild)
                .Select(c => c.GetComponent<DiceSlotUI>().Dice)
                .IndexOf(value);
            Player.Instance.HasDicesChoice = false;
            Player.Instance.ChangeDice(value.Data.name, diceIndex);

            _choiceParent.gameObject.SetActive(false);
            _deck.gameObject.SetActive(false);
            _info.SetActive(true);
        }

        public override void Hide(Action onDone = null) {
            base.Hide(() => {
                ClearItems();
                onDone?.Invoke();
            });
        }

        private void ClearItems() {
            foreach (Transform slot in _choiceParent) {
               DestroyImmediate(slot.gameObject);
            }

            foreach (Transform slot in _deckParent) {
               DestroyImmediate(slot.gameObject);
            }
        }

        private void OnReturnClick() {
            _return.OnClick.Unsubscribe(OnReturnClick);

            _onReturn();
        }
    }
}