using UnityEngine;

namespace _Game.Scripts {
    public class SoundHolder : MonoBehaviour
    {
        [SerializeField] private AudioSource _sfx;
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioClip[] _clips;

        public static SoundHolder Instance { get; private set; }

        public SoundHolder() {
            Instance = this;
        }

        public void PlaySound(string name, float volume = 1f) {
            // TODO
            foreach (var clip in _clips)
            {
                if (clip.name == name)
                {
                    _sfx.PlayOneShot(clip);
                }
            }
        }

        public void PlayMusic(string name, float volume = 1f) {
            // TODO
            foreach (var clip in _clips)
            {
                if (clip.name == name)
                {
                    _music.Stop();
                    _music.clip = clip;
                    _music.Play();
                }
            }
        }
    }
}