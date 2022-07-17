namespace _Game.Scripts.Data {
    public enum EItemType {
        Weapon,
        Defence,
        Interaction,  // Amulet/ring etc
        HealthPotion,
        Any  // Special type, slots of this type accept everything
    }

    public static class ItemTypeHelper {
        public static bool Accepts(this EItemType slotType, EItemType itemType) {
            return slotType == EItemType.Any || slotType == itemType;
        }
    }
}