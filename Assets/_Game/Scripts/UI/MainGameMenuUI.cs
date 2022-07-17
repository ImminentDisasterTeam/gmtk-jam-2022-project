using System.Linq;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class MainGameMenuUI : UIElement {
        [SerializeField] private GameButton _inn;
        [SerializeField] private GameButton _shop;
        [SerializeField] private GameButton _dungeon;
        [SerializeField] private InnUI _innUI;
        [SerializeField] private ShopUI _shopUI;
        [SerializeField] private DungeonStarter _dungeonStarter;

        private Rng _rng;

        private void Awake() {
            _inn.OnClick.Subscribe(OpenInn);
            _shop.OnClick.Subscribe(OpenShop);
            _dungeon.OnClick.Subscribe(StartDungeon);
        }

        public void Load(Rng rng) {
            _rng = rng;
        }

        private void OpenInn() {
            Hide(() => {
                _innUI.Load(_rng, () => {
                    _innUI.Hide(() => {
                        Show();
                    });
                });
                _innUI.Show();
            });
        }

        private void OpenShop() {
            Hide(() => {
                _shopUI.Load(() => {
                    _shopUI.Hide(() => {
                        Show();
                    });
                });
                _shopUI.Show();
            });
        }

        private void StartDungeon() {
            var suitableDungeons = DataHolder.Instance.GetDungeons()
                .Where(d => d.levels.Contains(Player.Instance.Level))
                .ToArray();

            if (suitableDungeons.Length == 0) {
                // TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO TODO GAME END?
            }

            var dungeon = _rng.NextChoice(suitableDungeons);

            Hide(() => {
                _dungeonStarter.StartDungeon(dungeon, _rng, finishedByDeath => {
                    // TODO show some death screen
                    Show();
                });
            });
        }
    }
}