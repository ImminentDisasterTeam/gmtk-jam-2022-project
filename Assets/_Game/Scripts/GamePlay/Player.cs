using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.UI;
using GeneralUtils;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.GamePlay {
    public class Player {
        private const string FilePath = "Saves/save.json";

        private PlayerSaveData _data;

        public static Player Instance { get; private set; }

        public StatsData AttackStats => ItemOrDefault(_data.attackSlot, _data.defaultAttack);
        public StatsData DefenceStats => ItemOrDefault(_data.defenceSlot, _data.defaultDefence);
        public StatsData InteractionStats => ItemOrDefault(_data.interactSlot, _data.defaultInteraction);

        public IEnumerable<Dice> Dices => _data.dices.Select(diceName => string.IsNullOrEmpty(diceName)
            ? null
            : new Dice(DataHolder.Instance.GetDices().First(dice => dice.name == diceName)));
        public IEnumerable<Item> Items => _data.inventory.Select(itemName => string.IsNullOrEmpty(itemName)
            ? null
            : new Item(DataHolder.Instance.GetItems().First(item => item.name == itemName)));

        public int EscapeThreshold {
            get => _data. escapeThreshold;
            private set {
                _data.escapeThreshold = value;
                WriteToSave();
            }
        }
        public int Level {
            get => _data. level;
            private set {
                _data.level = value;
                WriteToSave();
            }
        }

        public bool HasDicesChoice {
            get => _data.hasDicesChoice;
            set {
                _data.hasDicesChoice = value;
                WriteToSave();
            }
        }

        public void RaiseEscapeThreshold() {
            EscapeThreshold += DataHolder.Instance.GetSettings().escapeThresholdStep;
        }

        public int Health { get; private set; }
        public int MaxHealth => _data.maxHealth;
        public int Money {
            get => _data.money;
            set {
                _data.money = value;
                StatsPanelUI.Instance.SetMoney(value);
                WriteToSave();
            }
        }

        private bool _inDungeon;
        public bool InDungeon {
            get => _inDungeon;
            set {
                var wasInDungeon = _inDungeon;
                _inDungeon = value;

                if (!wasInDungeon && value) {
                    StatsPanelUI.Instance.SetEnabled(false);
                } else if (wasInDungeon && !value) {
                    StatsPanelUI.Instance.SetEnabled(true);
                    HasDicesChoice = true;
                    Level++;
                    ResetHealthToMax();
                    WriteToSave();
                }
            }
        }

        public static void LoadPlayer(Rng rng) {
            Instance = new Player();
            Instance.Initialize(rng);
        }

        private void Initialize(Rng rng) {
            _data = LoadFromSave() ?? CreateNewData(rng);
            PostLoad();
        }

        public void Reset(Rng rng, bool byDeath = false) {
            // TODO HANDLE DEATH
            _data = CreateNewData(rng);
            PostLoad();
        }

        private void PostLoad() {
            ResetHealthToMax();
            WriteToSave();
            StatsPanelUI.Instance.Load(MaxHealth, Money, _data.attackSlot, _data.defenceSlot, _data.interactSlot);
            InventoryPanelUI.Instance.Load(Items);
        }

        public bool ChangeHealth(int deltaHealth) {
            Health = Math.Min(Math.Max(0, Health + deltaHealth), MaxHealth);
            StatsPanelUI.Instance.SetHealth(Health);
            return Health == 0;
        }

        public void ChangeDice([CanBeNull] string diceName, int index) {
            _data.dices[index] = diceName;
            WriteToSave();
        }

        public void ChangeItem([CanBeNull] string itemName, int index) {
            _data.inventory[index] = itemName;
            WriteToSave();
        }

        public void ChangeAttackSlot([CanBeNull] string itemName) {
            _data.attackSlot = itemName;
            WriteToSave();
        }

        public void ChangeDefenceSlot([CanBeNull] string itemName) {
            _data.defenceSlot = itemName;
            WriteToSave();
        }

        public void ChangeInteractionSlot([CanBeNull] string itemName) {
            _data.defenceSlot = itemName;
            WriteToSave();
        }

        public void ResetHealthToMax() {
            Health = MaxHealth;
            StatsPanelUI.Instance.SetHealth(Health);
        }

        private static PlayerSaveData CreateNewData(Rng rng) {
            var players = DataHolder.Instance.GetPlayers();
            var playerData = rng.NextChoice(players);
            var settings = DataHolder.Instance.GetSettings();

            var data = new PlayerSaveData {
                maxHealth = playerData.maxHealth,
                image = playerData.image,
                escapeThreshold = settings.initialEscapeThreshold,
                level = 1,
                attackSlot = "",
                defenceSlot = "",
                interactSlot = "",
                defaultAttack = playerData.attack,
                defaultDefence = playerData.defence,
                defaultInteraction = playerData.interaction,
                dices = settings.initialDices,
                inventory = Array.Empty<string>(),
                hasDicesChoice = false
            };

            Array.Resize(ref data.dices, settings.deckSize);
            Array.Resize(ref data.inventory, settings.inventorySize);

            return data;
        }

        private void WriteToSave() {
            if (InDungeon) {
                return;
            }

            File.WriteAllText(FilePath, JsonUtility.ToJson(_data, true));
        }

        private static StatsData ItemOrDefault(string item, StatsData @default) {
            return string.IsNullOrEmpty(item)
                ? @default
                : DataHolder.Instance
                    .GetItems()
                    .First(i => i.name == item)
                    .stats;
        }

        private PlayerSaveData? LoadFromSave() {
            if (!File.Exists(FilePath)) {
                return null;
            }

            return JsonUtility.FromJson<PlayerSaveData>(File.ReadAllText(FilePath));
        }
    }
}