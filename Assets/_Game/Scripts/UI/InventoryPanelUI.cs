using _Game.Scripts.GamePlay;
using GeneralUtils;

namespace _Game.Scripts.UI {
    public class InventoryPanelUI : ItemPanelUI {
        public static InventoryPanelUI Instance { get; private set; }

        public InventoryPanelUI() {
            Instance = this;
        }
        
        protected override void OnItemSlotChanged(ItemSlotUI slot, Item oldValue, Item value) {
            var index = GetDiceSlots().IndexOf(slot);
            Player.Instance.ChangeItem(value?.Data.name ?? "", index);
        }
    }
}