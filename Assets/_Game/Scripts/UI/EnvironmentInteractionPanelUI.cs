using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using GeneralUtils.Processes;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class EnvironmentInteractionPanelUI : UIElement {
        [SerializeField] private DiceActionPanelUI _diceActionPanel;
        [SerializeField] private ItemPanelUI _dropPanel;
        [SerializeField] private TextMeshProUGUI _text;
        private RoomData _data;

        public bool CanRoll => _data.Type != ERoomType.Empty && _diceActionPanel.CanRoll;
        public bool CanContinue => _data.Type == ERoomType.Empty || CanRoll || (!_data.mandatory && _diceActionPanel.Empty);

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
                ERoomType.Empty => "Just peace and quiet.",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _data = data;
            if (data.Type == ERoomType.Empty) {
                return;
            }

            _diceActionPanel.Load(EActionType.Interact, interactionStats);
            _diceActionPanel.OnContentsChanged.Subscribe(OnPanelChanged);
        }

        private void OnPanelChanged(DiceActionPanelUI _) {
            _onContinueStatusChanged(CanContinue);
        }

        public (int deltaHealth, int deltaMoney) Roll(Rng rng) {
            var result = _diceActionPanel.Roll(rng);

            var (deltaHealth, healthDescription) = GetHealthRollResult(_data, result);
            var (deltaMoney, items, chestDescription) = GetChestRollResult(_data, result, rng);
            _text.text = _data.Type switch {
                ERoomType.ItemChest => chestDescription,
                ERoomType.GoldChest => chestDescription,
                ERoomType.Health => healthDescription,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (items.Length > 0) {
                _dropPanel.Load(items.Select(i => new Item(DataHolder.Instance.GetItems().First(d => d.name == i))));
                _dropPanel.Show();
            }

            return (deltaHealth, deltaMoney);
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

        private static (int gold, string[] items, string desription) GetChestRollResult(RoomData data, int result, Rng rng) {
            if (result < data.ChestCheck) {
                return (0, Array.Empty<string>(), "You got nothing.");
            }

            if (result < data.hiddenCheck) {
                return (data.gold, GetItems(data.contentPool), $"You got {LootType()}!");
            }

            var items = GetItems(data.contentPool).Concat(GetItems(data.hiddenContentPool)).ToArray();
            return (data.gold + data.hiddenGold, items, $"You got more {LootType()} than you expected!");

            string LootType() => data.Type == ERoomType.GoldChest ? "gold" : "items";
            string[] GetItems(PoolItemData[] pool) => (pool ?? Array.Empty<PoolItemData>()).Select(item => rng.NextChoice(item.subPool).type).ToArray();
        }

        private static (int health, string description) GetHealthRollResult(RoomData data, int result) {
            if ((data.healthChange?.Length ?? 0) == 0) {
                return (0, "");
            }

            for (var i = 0; i < data.healthChange.Length; i++) {
                if (Check(result, i, data.check)) {
                    return (data.healthChange[i], $"Received {Health(data.healthChange[i])}!");
                }
            }

            throw new ArgumentOutOfRangeException();

            static bool Check(int result, int index, int[] check) {
                if (index == 0) {
                    return result < check[0];
                }

                if (index == check.Length - 1) {
                    return result >= check[index];
                }

                return result < check[index + 1] && result >= check[index];
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

        public override void Show(Action onDone = null) {
            var showProcess = new ParallelProcess();
            showProcess.Add(new AsyncProcess(base.Show));
            if (_data.Type != ERoomType.Empty) {
                showProcess.Add(new AsyncProcess(_diceActionPanel.Show));
            }
            showProcess.Run(onDone);
        }

        public override void Hide(Action onDone = null) {
            _diceActionPanel.OnContentsChanged.Unsubscribe(OnPanelChanged);
            var hideProcess = new ParallelProcess();
            if (_data.Type != ERoomType.Empty) {
                hideProcess.Add(new AsyncProcess(_diceActionPanel.Hide));
            }
            if (_dropPanel.gameObject.activeSelf) {
                hideProcess.Add(new AsyncProcess(_dropPanel.Hide));
            }
            hideProcess.Add(new AsyncProcess(base.Hide));
            hideProcess.Run(onDone);
        }
    }
}