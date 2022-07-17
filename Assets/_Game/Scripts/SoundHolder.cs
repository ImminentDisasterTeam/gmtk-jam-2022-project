using UnityEngine;

namespace _Game.Scripts {
    public class SoundHolder : MonoBehaviour {
        [SerializeField] private AudioClip[] _clips;

        public static SoundHolder Instance { get; private set; }

        public SoundHolder() {
            Instance = this;
        }

        public void PlaySound(string name, float volume = 1f) {
            // TODO
        }

        public void PlayMusic(string name, float volume = 1f) {
            // TODO
        }
    }
}