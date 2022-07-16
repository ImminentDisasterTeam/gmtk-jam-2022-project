using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI {
    public class DungeonStarter : MonoBehaviour {
        [Header("Start group")]
        [SerializeField] private GameObject _startGroup;
        [SerializeField] private TMP_InputField _dungeonNumberInput;
        [SerializeField] private TMP_InputField _randomSeedInput;
        [SerializeField] private Button _startButton;
        [Header("Game UI")]
        [SerializeField] private GameObject _gameUI;

        private void Awake() {
            _startButton.onClick.AddListener(OnButtonClick);
            ToggleUI(true);
        }

        private void OnButtonClick() {
            var dungeonNumber = int.Parse(_dungeonNumberInput.text);
            var randomSeed = int.Parse(_randomSeedInput.text);

            var dungeons = DataHolder.Instance.GetDungeons();
            if (dungeons.Length <= dungeonNumber) {
                Debug.LogError($"Index {dungeonNumber} is out of range: only got {dungeons.Length} dungeons!");
                return;
            }

            var rng = new Rng(randomSeed);
            var dungeon = new Dungeon(dungeons[dungeonNumber], rng);
            new DungeonRunner(dungeon, () => ToggleUI(true));
            ToggleUI(false);
        }

        private void ToggleUI(bool isStartingUI) {
            _startGroup.SetActive(isStartingUI);
            _gameUI.SetActive(!isStartingUI);
        }
    }
}