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

        public bool merchant;

        public override EType Type => EType.Item;

        protected override void PerformSwapWith(SlotUI other) {
            var otherSlot = (ItemSlotUI) other;

            if ((merchant && !otherSlot.merchant) || (!merchant && otherSlot.merchant)) {
                var merchantPrice = (merchant ? Item : otherSlot.Item)?.Data.GetPrice(true) ?? 0;
                var playerPrice = (merchant ? otherSlot.Item : Item)?.Data.GetPrice(false) ?? 0;
                Player.Instance.Money += playerPrice - merchantPrice;
            }

            (otherSlot.Item, Item) = (Item, otherSlot.Item);
        }

        protected override void LoadTooltip(Tooltip tooltip) {
            var itemTooltip = (ItemTooltip) tooltip;
            itemTooltip.Load(Item, merchant);
        }

        protected override bool IsEmpty() => Item == null;

        public override bool CanTakeFrom(SlotUI other) {
            var otherSlot = (ItemSlotUI) other;
            var can = otherSlot.Item == null || _itemType.Accepts(otherSlot.Item.Data.Type);
            if (!can || !(merchant && !otherSlot.merchant)) {
                return can;
            }

            var thisPrice = Item?.Data.GetPrice(true) ?? 0;
            var otherPrice = otherSlot.Item?.Data.GetPrice(false) ?? 0;
            return Player.Instance.Money + otherPrice - thisPrice >= 0;
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