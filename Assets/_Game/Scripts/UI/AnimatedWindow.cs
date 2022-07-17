using System;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class AnimatedWindow : UIElement {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameButton _return;
        
        private Action _onReturn;

        private void Awake() {
            _return.OnClick.Subscribe(OnReturn);
        }

        private void OnReturn() {
            _onReturn();
        }

        public void Load(Action onReturn) {
            _onReturn = onReturn;
        }

        protected virtual void OnOpen() { }

        public override void Show(Action onDone = null) {
            base.Show(() => {
                _animator.Play("animation");
                OnOpen();
                onDone?.Invoke();
            });
        }
    }
}