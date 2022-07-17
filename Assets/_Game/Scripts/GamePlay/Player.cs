using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Scripts.Data;
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

        public int Health { get; private set; }
        public int MaxHealth => _data.maxHealth;
        public int Money {
            get => _data.money;
            set {
                _data.money = value;
                WriteToSave();
            }
        }

        public static void LoadPlayer(Rng rng) {
            Instance = new Player();
            Instance.Initialize(rng);
        }

        private void Initialize(Rng rng) {
            _data = LoadFromSave() ?? CreateNewData(rng);
            ResetHealthToMax();
            WriteToSave();
        }

        public void Reset(Rng rng) {
            _data = CreateNewData(rng);
            ResetHealthToMax();
            WriteToSave();
        }

        public bool ChangeHealth(int deltaHealth) {
            Health = Math.Min(Math.Max(0, Health + deltaHealth), MaxHealth);
            return Health == 0;
        }

        public void ChangeDice([CanBeNull] string diceName, int index) {
            _data.dices[index] = diceName;
            WriteToSave();
        }

        public void ResetHealthToMax() {
            Health = MaxHealth;
        }

        private static PlayerSaveData CreateNewData(Rng rng) {
            var players = DataHolder.Instance.GetPlayers();
            var playerData = rng.NextChoice(players);
            var settings = DataHolder.Instance.GetSettings();

            var data = new PlayerSaveData {
                maxHealth = playerData.maxHealth,
                image = playerData.image,
                attackSlot = "",
                defenceSlot = "",
                interactSlot = "",
                defaultAttack = playerData.attack,
                defaultDefence = playerData.defence,
                defaultInteraction = playerData.interaction,
                dices = settings.initialDices,
                inventory = Array.Empty<string>()
            };

            Array.Resize(ref data.dices, settings.deckSize);

            return data;
        }

        private void WriteToSave() {
            File.WriteAllText(FilePath, JsonUtility.ToJson(_data));
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