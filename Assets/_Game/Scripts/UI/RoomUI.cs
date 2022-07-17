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
        [SerializeField] private ActionSelectionPanelUI _actionSelectionPanel;
        [SerializeField] private DiceActionPanelUI _playerActionPanel;
        [SerializeField] private EnemyUI _enemyUI;
        [SerializeField] private GameObject _battleUI;

        // private RoomData _data;
        private Rng _rng;
        private Action<bool> _onRoomFinished;

        public void StartRoom(RoomData data, Rng rng, Action<bool> onRoomFinished) {
            // _data = data;
            _rng = rng;
            _onRoomFinished = onRoomFinished;

            Show(() => {
                switch (data.Type) {
                    case ERoomType.Battle:
                        StartBattleRoom(data, rng);
                        break;
                    case ERoomType.ItemChest:
                    case ERoomType.GoldChest:
                    case ERoomType.Health:
                    case ERoomType.Empty:
                        StartEnvironmentRoom(data);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        private void StartBattleRoom(RoomData data, Rng rng) {
            _battleUI.SetActive(true);
            // EnableContinue(false);

            _actionSelectionPanel.Load();
            _actionSelectionPanel.Show();
            _continuePanel.Show();

            var lastStatus = false;

            new Battle(data, rng, Reload, _enemyUI, out var onPlayerReady, EnableContinue,
                _continuePanel.OnContinue, GetPlayerRoll, DisableInput, FinishRoom);  // TODO do not finish room this way. Do it manually

            void Reload() {
                lastStatus = false;
                _actionSelectionPanel.Reload();
            }

            void DisableInput() {
                _actionSelectionPanel.SetEnabled(false);
            }
            
            void CheckReady() {
                var status = _playerActionPanel.CanRoll;
                if (!lastStatus && status) {
                    onPlayerReady(true);
                } else if (lastStatus && !status) {
                    onPlayerReady(false);
                }

                lastStatus = status;
            }

            _actionSelectionPanel.OnSelected.Subscribe(CheckReady);
            _playerActionPanel.OnContentsChanged.Subscribe(_ => CheckReady());

            void EnableContinue(bool enabled) {
                _continuePanel.SetEnabled(enabled);
            }
            
            (EActionType, int) GetPlayerRoll() {
                var result = _playerActionPanel.Roll(_rng);
                return (_actionSelectionPanel.CurrentType!.Value, result);
            }
        }

        private void StartEnvironmentRoom(RoomData data) {
            _environmentInteractionPanel.Load(data, Player.Instance.InteractionStats);
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
            _playerActionPanel.OnContentsChanged.ClearSubscribers();
            _actionSelectionPanel.OnSelected.ClearSubscribers();

            _battleUI.SetActive(false);

            var hideProcess = new ParallelProcess();
            hideProcess.Add(new AsyncProcess(_continuePanel.Hide));
            if (_environmentInteractionPanel.gameObject.activeSelf) {
                hideProcess.Add(new AsyncProcess(_environmentInteractionPanel.Hide));
            }
            if (_actionSelectionPanel.gameObject.activeSelf) {
                hideProcess.Add(new AsyncProcess(_actionSelectionPanel.Hide));
            }
            hideProcess.Add(new AsyncProcess(Hide));
            hideProcess.Run(() => {
                _onRoomFinished(finishByDeath);
            });
        }
    }
}