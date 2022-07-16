using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.UI
{
    public class ButtonInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _image;
        [SerializeField] private Sprite _default, _pressed;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _pressAudio, _releaseAudio;

        public void OnPointerDown(PointerEventData eventData)
        {
            _image.sprite = _pressed;
            _audioSource.PlayOneShot(_pressAudio);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _image.sprite = _default;
            _audioSource.PlayOneShot(_releaseAudio);
        }
    }
}
