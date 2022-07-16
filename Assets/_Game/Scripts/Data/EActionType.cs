namespace _Game.Scripts.Data {
    public enum EActionType {
        Attack,
        Defend,
        Charge,
        Heal,
        UseItem,  // HEAL, BUT FOR PLAYER -> no serialization
        Wait,
        Interact   // FOR INTERACTING WITH ENVIRONMENT -> no serialization
    }
}