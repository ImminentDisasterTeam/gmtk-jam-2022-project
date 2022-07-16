using System;
using _Game.Scripts.Data;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class SpriteHolder : MonoBehaviour {
        public static SpriteHolder Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        public Sprite GetSprite(string spriteName) {
            return null;  // TODO
        }

        public Sprite GetSprite(EActionType actionType) {
            var spriteName = actionType switch {
                EActionType.Attack => "attack.png",
                EActionType.Defend => "defend.png",
                EActionType.Charge => "charge.png",
                EActionType.Heal => "heal.png",
                EActionType.UseItem => "useItem.png",
                EActionType.Wait => "wait.png",
                EActionType.Interact => "interact.png",
                _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
            };

            return GetSprite(spriteName);
        }
    }
}