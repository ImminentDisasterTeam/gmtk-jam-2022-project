using System;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class SlotManager : MonoBehaviour {
        public static SlotManager Instance { get; private set; }

        [Header("DEBUG")]
        [SerializeField] private SlotUI _selectedSlot;  // serialized for debug purposes

        private SlotUI _currentFrameSlot;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (Input.GetMouseButtonUp(0) && _currentFrameSlot == null) {
                OnClick(null);
            }

            _currentFrameSlot = null;
        }

        public void OnClick([CanBeNull] SlotUI slot) {
            if (slot == null) {
                Debug.LogWarning("no slot clicked");
                if (_selectedSlot != null) {
                    _selectedSlot.ToggleSelection(false);
                    _selectedSlot = null;
                }

                return;
            }

            Debug.LogWarning($"{slot.Type} slot clicked");
            _currentFrameSlot = slot;

            if (_selectedSlot == null) {
                _selectedSlot = slot;
                _selectedSlot.ToggleSelection(true);
                return;
            }

            if (_selectedSlot.Type != slot.Type
                || !(_selectedSlot.State.CanTake() && slot.State.CanPut())) {
                _selectedSlot.ToggleSelection(false);
                _selectedSlot = null;
                return;
            }

            _selectedSlot.SwapWith(slot);
            _selectedSlot.ToggleSelection(false);
            _selectedSlot = null;
        }
    }
}