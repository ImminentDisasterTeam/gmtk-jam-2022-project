using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class ItemPanelUI : UIElement {
        [SerializeField] private Transform _itemSlotsParent;
        [SerializeField] private ItemSlotUI _itemSlotPrefab;

        public void Load(IEnumerable<Item> items) {
            ClearSlots();

            foreach (var item in items) {
                var itemSlot = Instantiate(_itemSlotPrefab, _itemSlotsParent);
                itemSlot.State = SlotUI.EState.Unlocked;
                itemSlot.OnContentsChanged.Subscribe(OnItemSlotChanged);
                itemSlot.Item = item;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
        }

        protected virtual void OnItemSlotChanged(ItemSlotUI slot, Item oldValue, Item value) { }

        private void ClearSlots() {
            foreach (var slot in GetDiceSlots()) {
                slot.OnContentsChanged.Unsubscribe(OnItemSlotChanged);
                DestroyImmediate(slot.gameObject);
            }
        }

        protected ItemSlotUI[] GetDiceSlots() {
            return Enumerable
                .Range(0, _itemSlotsParent.childCount)
                .Select(_itemSlotsParent.GetChild)
                .Select(t => t.GetComponent<ItemSlotUI>())
                .Where(c => c != null)
                .ToArray();
        }
    }
}