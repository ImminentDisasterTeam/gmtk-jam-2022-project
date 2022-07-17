using DG.Tweening;
using GeneralUtils.Processes;
using PlasticGui.Help.Conditions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI {
    public abstract class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private GameObject _selection;
        [SerializeField] private Tooltip _tooltip;

        private Process _tooltipProcess;
        
        public EState State;
        public abstract EType Type { get; }

        public void SwapWith(SlotUI other) {
            HideTooltip();
            PerformSwapWith(other);
        }
        protected abstract void PerformSwapWith(SlotUI other);

        public void ToggleSelection(bool selected) {
            if (_selection == null) {
                return;
            }

            _selection.SetActive(selected);
        }

        public void OnPointerClick(PointerEventData eventData) {
            SlotManager.Instance.OnClick(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Log("Enter");
            ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData) {
            Log("Exit");
            HideTooltip();
        }

        private void ShowTooltip() {
            if (IsEmpty()) {
                return;
            }
            Log("Show");

            _tooltipProcess?.TryAbort();
            LoadTooltip(_tooltip);
            var proc = new SerialProcess();
            proc.Add(new AsyncProcess(onDone => DOVirtual.DelayedCall(0.4f, () => onDone())));
            proc.Add(new AsyncProcess(_tooltip.Show));
            _tooltipProcess = proc;
            _tooltipProcess.Run();
        }

        private void HideTooltip() {
            if (IsEmpty()) {
                return;
            }
            Log("Hide");

            _tooltipProcess?.TryAbort();
            _tooltipProcess = new AsyncProcess(_tooltip.Hide);
            _tooltipProcess.Run();
        }

        protected abstract void LoadTooltip(Tooltip tooltip);

        protected abstract bool IsEmpty();

        private void Log(string message) => Debug.Log($"{name}: {message}");

        public enum EState {
            Locked,
            CanTake,
            CanPut,
            Unlocked
        }

        public enum EType {
            Item,
            Dice
        }

        public virtual bool CanTakeFrom(SlotUI other) {
            return true;
        }
    }

    public static class SlotUIStateHelper {
        public static bool CanTake(this SlotUI.EState state) {
            return state == SlotUI.EState.CanTake || state == SlotUI.EState.Unlocked;
        }

        public static bool CanPut(this SlotUI.EState state) {
            return state == SlotUI.EState.CanPut || state == SlotUI.EState.Unlocked;
        }
    }
}