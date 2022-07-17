using System;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using GeneralUtils;
using GeneralUtils.Processes;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class RoomUI : UIElement {
        [SerializeField] private ContinuePanelUI _continuePanel;
        [SerializeField] private EnvironmentInteractionPanelUI _environmentInteractionPanel;

        private RoomData _data;
        private Rng _rng;
        private Action<bool> _onRoomFinished;

        public void StartRoom(RoomData data, Rng rng, Action<bool> onRoomFinished) {
            _data = data;
            _rng = rng;
            _onRoomFinished = onRoomFinished;

            Show(() => {
                switch (data.Type) {
                    case ERoomType.Battle:
                        StartBattleRoom();
                        break;
                    case ERoomType.ItemChest:
                    case ERoomType.GoldChest:
                    case ERoomType.Health:
                    case ERoomType.Empty:
                        StartEnvironmentRoom();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private void StartBattleRoom() {
            // TODO
        }

        private void StartEnvironmentRoom() {
            _environmentInteractionPanel.Load(_data, Player.Instance.InteractionStats);
            _environmentInteractionPanel.OnContinueStatusChanged.ClearSubscribers();
            _environmentInteractionPanel.OnContinueStatusChanged.Subscribe(OnEnvironmentContinueStatusChanged);

            _continuePanel.SetEnabled(_environmentInteractionPanel.CanContinue);
            _continuePanel.OnContinue.Subscribe(OnEnvironmentContinue);

            _environmentInteractionPanel.Show();
            _continuePanel.Show();
        }

        private void OnEnvironmentContinueStatusChanged(bool canContinue) {
            _continuePanel.SetEnabled(canContinue);
        }

        private void OnEnvironmentContinue() {
            if (!_environmentInteractionPanel.CanRoll) {
                FinishRoom();
                return;
            }

            var (deltaHealth, deltaMoney) = _environmentInteractionPanel.Roll(_rng);
            Player.Instance.Money += deltaMoney;
            var dead = Player.Instance.ChangeHealth(deltaHealth);
            if (dead) {
                FinishRoom(true);
            }
        }

        private void FinishRoom(bool finishByDeath = false) {
            _environmentInteractionPanel.OnContinueStatusChanged.ClearSubscribers();
            _continuePanel.OnContinue.ClearSubscribers();

            var hideProcess = new ParallelProcess();
            hideProcess.Add(new AsyncProcess(_continuePanel.Hide));
            if (_environmentInteractionPanel.gameObject.activeSelf) {
                hideProcess.Add(new AsyncProcess(_environmentInteractionPanel.Hide));
            }
            hideProcess.Add(new AsyncProcess(Hide));
            hideProcess.Run(() => {
                _onRoomFinished(finishByDeath);
            });
        }
    }
}