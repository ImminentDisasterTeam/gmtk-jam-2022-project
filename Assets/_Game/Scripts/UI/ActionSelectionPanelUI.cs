using System;
using _Game.Scripts.Data;
using _Game.Scripts.GamePlay;
using UnityEngine;
using Event = GeneralUtils.Event;

namespace _Game.Scripts.UI {
    public class ActionSelectionPanelUI : UIElement {
        [SerializeField] private GameButton _attackButton;
        [SerializeField] private GameButton _defendButton;
        [SerializeField] private GameButton _useItemButton;
        [SerializeField] private GameButton _runAwayButton;
        [SerializeField] private DiceActionPanelUI _playerActionPanel;

        public EActionType? CurrentType { get; private set; }

        private readonly Action _onSelected;
        public Event OnSelected { get; }

        public ActionSelectionPanelUI() {
            OnSelected = new Event(out _onSelected);
        }

        private void Awake() {
            _attackButton.OnClick.Subscribe(OnAttack);
            _defendButton.OnClick.Subscribe(OnDefend);
            _useItemButton.OnClick.Subscribe(OnUseItem);
            _runAwayButton.OnClick.Subscribe(OnRunAway);
            _playerActionPanel.OnContentsChanged.Subscribe(OnActionPanelChanged);
        }

        public void Reload() {
            InfoPanelUI.Instance.Hide();
            CurrentType = null;
            SetEnabled(true);
            if (_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Hide();
            }
        }

        private void OnAttack() {
            InfoPanelUI.Instance.Hide();
            CurrentType = EActionType.Attack;
            _playerActionPanel.Load(EActionType.Attack, Player.Instance.AttackStats);
            if (!_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Show();
            }

            _onSelected();
        }

        private void OnDefend() {
            InfoPanelUI.Instance.Hide();
            CurrentType = EActionType.Defend;
            _playerActionPanel.Load(EActionType.Defend, Player.Instance.DefenceStats);
            if (!_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Show();
            }

            _onSelected();
        }

        private void OnUseItem() {
            // TODO Useitem
        }

        private void OnRunAway() {
            CurrentType = EActionType.ChickenOut;
            _playerActionPanel.Load(EActionType.ChickenOut, Player.Instance.InteractionStats);
            if (!_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Show();
            }

            _onSelected();

            InfoPanelUI.Instance.LoadEscape(Player.Instance.EscapeThreshold);
            InfoPanelUI.Instance.Show();
        }

        private void OnActionPanelChanged(DiceActionPanelUI actionPanel) {
            if (!actionPanel.Empty) {
                SetEnabled(false);
            }
        }

        public void SetEnabled(bool enabled) {
            _attackButton.SetEnabled(enabled);
            _defendButton.SetEnabled(enabled);
            _useItemButton.SetEnabled(enabled);
            _runAwayButton.SetEnabled(enabled);
        }
    }
}