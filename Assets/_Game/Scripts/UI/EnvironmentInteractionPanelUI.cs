using System;
using System.Collections.Generic;
using _Game.Scripts.Data;
using GeneralUtils;
using GeneralUtils.Processes;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class EnvironmentInteractionPanelUI : UIElement {
        [SerializeField] private DiceActionPanelUI _diceActionPanel;
        [SerializeField] private TextMeshProUGUI _text;
        private RoomData _data;

        public bool CanContinue => _diceActionPanel.CanRoll || (!_data.mandatory && _diceActionPanel.Empty);
        private readonly Action<bool> _onContinueStatusChanged;
        public Event<bool> OnContinueStatusChanged { get; }

        public EnvironmentInteractionPanelUI() {
            OnContinueStatusChanged = new Event<bool>(out _onContinueStatusChanged);
        }

        public void Load(RoomData data, StatsData interactionStats) {
            _text.text = data.Type switch {
                ERoomType.ItemChest => GetChestDescription(data),
                ERoomType.GoldChest => GetChestDescription(data),
                ERoomType.Health => GetHealthDescription(data),
                _ => throw new ArgumentOutOfRangeException()
            };

            _diceActionPanel.Load(EActionType.Interact, interactionStats);
            _diceActionPanel.OnContentsChanged.Subscribe(OnPanelChanged);
            _data = data;
        }

        private void OnPanelChanged(DiceActionPanelUI _) {
            _onContinueStatusChanged(CanContinue);
        }

        public (int healthChange, int moneyChange) Continue(Rng rng) {
            var result = _diceActionPanel.Roll(rng);
            var items = GetItems(_data, result, rng);
            // TODO show optional selectionPanel
            return (GetHealthChange(_data, result), GetGold(_data, result));
        }

        private static string GetChestDescription(RoomData data) {
            return $"Get {data.ChestCheck} or more to acquire loot.";
        }

        private static string GetHealthDescription(RoomData data) {
            var lines = new List<string>();
            for (var i = 0; i < data.healthChange.Length; i++) {
                lines.Add($"Get {Check(i, data.check)} to receive {Health(data.healthChange[i])}.");
            }

            return string.Join(" ", lines);

            static string Check(int index, int[] check) {
                if (index == 0) {
                    return $"less then {check[index]}";
                }

                return $"{check[index - 1]} or more";
            }

            static string Health(int healthChange) {
                if (healthChange == 0) {
                    return "nothing";
                }

                if (healthChange > 0) {
                    return $"{healthChange} health";
                }

                return $"{-healthChange} damage";
            }
        }

        private static int GetHealthChange(RoomData data, int result) {
            // var healthChange = _data.Type != ERoomType.Health
            //     ? 0
            //     :
            return 0; // TODO
        }

        private static int GetGold(RoomData data, int result) {
            return 0; // TODO
        }

        private static List<ItemData> GetItems(RoomData data, int result, Rng rng) {
            return new List<ItemData>(); // TODO
        }

        public override void Show(Action onDone = null) {
            var showProcess = new ParallelProcess();
            showProcess.Add(new AsyncProcess(base.Show));
            showProcess.Add(new AsyncProcess(_diceActionPanel.Show));
            showProcess.Run(onDone);
        }

        public override void Hide(Action onDone = null) {
            _diceActionPanel.OnContentsChanged.Unsubscribe(OnPanelChanged);
            var hideProcess = new ParallelProcess();
            hideProcess.Add(new AsyncProcess(_diceActionPanel.Hide));
            hideProcess.Add(new AsyncProcess(base.Hide));
            hideProcess.Run(onDone);
        }
    }
}