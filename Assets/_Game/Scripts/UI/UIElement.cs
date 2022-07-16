using System;
using UnityEngine;

namespace _Game.Scripts.UI {
    public abstract class UIElement : MonoBehaviour {
        public virtual void Show(Action onDone = null) {
            gameObject.SetActive(true);
            onDone?.Invoke();
        }

        public virtual void Hide(Action onDone = null) {
            gameObject.SetActive(false);
            onDone?.Invoke();
        }
    }
}