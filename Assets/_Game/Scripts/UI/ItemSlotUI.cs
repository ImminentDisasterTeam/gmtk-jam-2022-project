using System;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class ItemSlotUI : SlotUI {
        [SerializeField] private Image _itemImage;
        [SerializeField] private EItemType _itemType = EItemType.Any;
        // TODO tooltip

        public override EType Type { get; }

        public override void SwapWith(SlotUI other) {
            var otherSlot = (ItemSlotUI) other;
            (otherSlot.Item, Item) = (Item, otherSlot.Item);
        }

        public override bool CanTakeFrom(SlotUI other) {
            var otherSlot = (ItemSlotUI) other;
            return otherSlot.Item == null || _itemType.Accepts(otherSlot.Item.Data.Type);
        }

        private readonly Action<ItemSlotUI, Item, Item> _onContentsChanged;
        public Event<ItemSlotUI, Item, Item> OnContentsChanged { get; }

        [CanBeNull] private Item _item;
        [CanBeNull]
        public Item Item {
            get => _item;
            set {
                var oldValue = _item;
                _item = value;

                _itemImage.gameObject.SetActive(false);

                if (!(_item is { } item)) {
                    _onContentsChanged(this, oldValue, null);
                    return;
                }

                _itemImage.sprite = SpriteHolder.Instance.GetSprite(item.Data.image);
                _itemImage.gameObject.SetActive(true);

                _onContentsChanged(this, oldValue, value);
            }
        }

        public ItemSlotUI() {
            OnContentsChanged = new Event<ItemSlotUI, Item, Item>(out _onContentsChanged);
        }
    }
}