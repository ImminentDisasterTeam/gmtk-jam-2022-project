using _Game.Scripts.Data;

namespace _Game.Scripts.GamePlay {
    public class Item {
        public ItemData Data { get; }

        public Item(ItemData data) {
            Data = data;
        }
    }
}