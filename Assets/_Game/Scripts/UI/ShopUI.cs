using System;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class ShopUI : UIElement {
        [SerializeField] private Transform _slotsParent;
        [SerializeField] private ItemSlotUI _slotPrefab;
        [SerializeField] private GameButton _return;

        private Action _onReturn;

        private void Awake() {
            _return.OnClick.Subscribe(OnReturn);
        }

        private void OnReturn() {
            _onReturn();
        }

        public void Load(Action onReturn) {
            _onReturn = onReturn;
            Clear();

            foreach (var itemName in DataHolder.Instance.GetSettings().shopInventory) {
                var item = new Item(DataHolder.Instance.GetItems().First(i => i.name == itemName));
                CreateSlot().Item = item;
            }

            for (var i = 0; i < DataHolder.Instance.GetSettings().inventorySize; i++) {
                CreateSlot();
            }

            ItemSlotUI CreateSlot() {
                var slot = Instantiate(_slotPrefab, _slotsParent);
                slot.merchant = true;
                slot.State = SlotUI.EState.Unlocked;
                return slot;
            }
        }

        public override void Hide(Action onDone = null) {
            base.Hide(() => {
                Clear();
                onDone?.Invoke();
            });
        }

        private void Clear() {
            foreach (Transform slot in _slotsParent) {
                DestroyImmediate(slot.gameObject);
            }
        }
    }
}