using System;
using System.Collections.Generic;
using _Game.Scripts.Data;
using GeneralUtils;
using UnityEngine;

namespace _Game.Scripts.UI {
    public class SpriteHolder : MonoBehaviour {
        [SerializeField] private Texture2D[] _spriteSheets;

        public static SpriteHolder Instance { get; private set; }
        private readonly Dictionary<string, Sprite> _cache = new Dictionary<string, Sprite>();

        private void Awake() {
            Instance = this;

            foreach (var spriteSheet in _spriteSheets) {
                var sprites = Resources.LoadAll<Sprite>(spriteSheet.name);
                foreach (var sprite in sprites) {
                    _cache.Add(sprite.name, sprite);
                }
            }
        }

        public Sprite GetSprite(string spriteName) {
            return _cache.GetValue(spriteName, () => Resources.Load<Sprite>(spriteName));
        }

        public Sprite GetSprite(EActionType actionType) {
            var spriteName = actionType switch {
                EActionType.Attack => "attack",
                EActionType.Defend => "defend",
                EActionType.Charge => "charge",
                EActionType.Heal => "heal",
                EActionType.UseItem => "useItem",
                EActionType.Wait => "wait",
                EActionType.Interact => "interact",
                EActionType.ChickenOut => "escape",
                _ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
            };

            return GetSprite(spriteName);
        }
    }
}