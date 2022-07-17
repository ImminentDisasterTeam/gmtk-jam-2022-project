using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.UI {
    public abstract class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private GameObject _selection;

        public EState State;
        public abstract EType Type { get; }

        public abstract void SwapWith(SlotUI other);

        public void ToggleSelection(bool selected) {
            if (_selection == null) {
                return;
            }

            _selection.SetActive(selected);
        }

        public void OnPointerClick(PointerEventData eventData) {
            SlotManager.Instance.OnClick(this);
            Log("Click");
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Log("Enter");
        }

        public void OnPointerExit(PointerEventData eventData) {
            Log("Exit");
        }

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