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
            CurrentType = null;
            SetEnabled(true);
            if (_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Hide();
            }
        }

        private void OnAttack() {
            CurrentType = EActionType.Attack;
            _playerActionPanel.Load(EActionType.Attack, Player.Instance.AttackStats);
            if (!_playerActionPanel.gameObject.activeSelf) {
                _playerActionPanel.Show();
            }

            _onSelected();
        }

        private void OnDefend() {
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
            // TODO RunAway
        }

        private void OnActionPanelChanged(DiceActionPanelUI actionPanel) {
            if (!actionPanel.Empty) {
                SetEnabled(false);
            }
        }

        private void SetEnabled(bool enabled) {
            _attackButton.SetEnabled(enabled);
            _defendButton.SetEnabled(enabled);
            _useItemButton.SetEnabled(enabled);
            _runAwayButton.SetEnabled(enabled);
        }
    }
}