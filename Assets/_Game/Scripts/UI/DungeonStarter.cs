using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Game.Scripts.UI {
    public class DungeonStarter : MonoBehaviour {
        [Header("Start group")]
        [SerializeField] private GameObject _startGroup;
        [SerializeField] private TMP_InputField _dungeonNumberInput;
        [SerializeField] private TMP_InputField _randomSeedInput;
        [SerializeField] private Button _startButton;
        [Header("Game UI")]
        [SerializeField] private GameObject _gameUI;
        [SerializeField] private RoomUI _roomUI;
        [SerializeField] private DungeonDiceUI _dungeonDiceUI;

        private void Start() {
            _startButton.onClick.AddListener(OnButtonClick);
            ToggleUI(true);
            Player.LoadPlayer(new Rng(42));
        }

        private void OnButtonClick() {
            var dungeonNumber = int.Parse(_dungeonNumberInput.text);
            var randomSeed = int.Parse(_randomSeedInput.text);

            var dungeons = DataHolder.Instance.GetDungeons();
            if (dungeons.Length <= dungeonNumber) {
                Debug.LogError($"Index {dungeonNumber} is out of range: only got {dungeons.Length} dungeons!");
                return;
            }

            Player.Instance.InDungeon = true;

            var rng = new Rng(randomSeed);
            var dungeon = new Dungeon(dungeons[dungeonNumber]);
            new DungeonRunner(dungeon, rng, _roomUI.StartRoom, finishedByDeath => {
                Debug.LogWarning("FINISHED DUNGEON." + (finishedByDeath ? " DIED." : ""));

                Player.Instance.InDungeon = false;
                if (finishedByDeath) {
                    Player.Instance.Reset(rng);
                }

                _dungeonDiceUI.Hide();
                ToggleUI(true);
            });

            ToggleUI(false);
            _dungeonDiceUI.Load(Player.Instance.Dices, DataHolder.Instance.GetSettings().handSize, rng);
            _dungeonDiceUI.Show();
        }

        private void ToggleUI(bool isStartingUI) {
            _startGroup.SetActive(isStartingUI);
            _gameUI.SetActive(!isStartingUI);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DungeonStarter))]
    public class DungeonStarterEditor : Editor {
        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            // var dungeonStarter = (DungeonStarter) target;

            if (GUILayout.Button("Reset Player")) {
                var rng = new Rng(42);
                Player.LoadPlayer(rng);
                Player.Instance.Reset(rng);
            }
        }
    }
#endif
}