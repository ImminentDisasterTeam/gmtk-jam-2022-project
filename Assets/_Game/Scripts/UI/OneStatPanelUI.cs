using System;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class OneStatPanelUI : MonoBehaviour {
        [SerializeField] private ItemSlotUI _itemSlot;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private GameObject _diceGroup;
        [SerializeField] private TextMeshProUGUI _diceCount;

        private Func<StatsData> _statsGetter;
        private Action<string> _onChange;

        public void Load(Item item, Func<StatsData> statsGetter, Action<string> onChange) {
            _statsGetter = statsGetter;
            _onChange = onChange;

            _itemSlot.Item = item;
            _itemSlot.OnContentsChanged.ClearSubscribers();
            _itemSlot.OnContentsChanged.Subscribe(OnItemSlotChanged);
            SetEnabled(true);
            UpdateUI();
        }

        private void OnItemSlotChanged(ItemSlotUI slot, Item oldValue, Item value) {
            _onChange(value?.Data.name);
            UpdateUI();
        }

        private void UpdateUI() {
            var statsToDisplay = _statsGetter();
            _label.text = statsToDisplay.ToString();
            _diceGroup.SetActive(statsToDisplay.DiceCount != 0);
            _diceCount.text = statsToDisplay.DiceCount.ToString();
        }

        public void SetEnabled(bool enabled) {
            _itemSlot.State = enabled ? SlotUI.EState.Unlocked : SlotUI.EState.Locked;
        }
    }
}