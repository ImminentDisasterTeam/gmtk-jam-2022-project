using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class StatsPanelUI : UIElement {
        [SerializeField] private OneStatPanelUI _attackStatPanel;
        [SerializeField] private OneStatPanelUI _defenceStatPanel;
        [SerializeField] private OneStatPanelUI _interactionStatPanel;
        [SerializeField] private TextMeshProUGUI _healthLabel;
        [SerializeField] private TextMeshProUGUI _moneyLabel;
        private int _maxHealth;

        public static StatsPanelUI Instance { get; private set; }

        public StatsPanelUI() {
            Instance = this;
        }

        public void Load(int maxHealth, int money, string attackSlotItem, string defenceSlotItem, string interactionSlotItem) {
            _maxHealth = maxHealth;

            _attackStatPanel.Load(ItemOrNull(attackSlotItem), () => Player.Instance.AttackStats, Player.Instance.ChangeAttackSlot);
            _defenceStatPanel.Load(ItemOrNull(defenceSlotItem), () => Player.Instance.DefenceStats, Player.Instance.ChangeDefenceSlot);
            _interactionStatPanel.Load(ItemOrNull(interactionSlotItem), () => Player.Instance.InteractionStats, Player.Instance.ChangeInteractionSlot);

            SetHealth(maxHealth);
            SetMoney(money);
        }

        private static Item ItemOrNull([CanBeNull] string itemName) {
            return string.IsNullOrEmpty(itemName)
                ? null
                : new Item(DataHolder.Instance.GetItems().First(i => i.name == itemName));
        }

        public void SetHealth(int health) {
            _healthLabel.text = $"HP:   {health} / {_maxHealth}";
        }

        public void SetMoney(int money) {
            _moneyLabel.text = money.ToString();
        }

        public void SetEnabled(bool enabled) {
            _attackStatPanel.SetEnabled(enabled);
            _defenceStatPanel.SetEnabled(enabled);
            _interactionStatPanel.SetEnabled(enabled);
        }
    }
}