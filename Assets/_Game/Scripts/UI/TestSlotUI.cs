namespace _Game.Scripts.UI {
    public class TestSlotUI : SlotUI {
        public override EType Type => EType.Dice;

        public override void SwapWith(SlotUI other) {
            var otherSlot = (TestSlotUI) other;
            (otherSlot.name, name) = (name, otherSlot.name);
        }
    }
}