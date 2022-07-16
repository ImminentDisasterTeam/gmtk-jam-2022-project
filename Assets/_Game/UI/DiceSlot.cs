using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Game.UI
{
    public class DiceSlot : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _img;
        [SerializeField] private TextMeshProUGUI _text;
        private bool _diceSelected = false;
        private int _diceValue = 0;

        private void SetDice(int value)
        {
            _img.gameObject.SetActive(true);
            _diceValue = value;
            _text.text = "D" + _diceValue;
            //load sprite here
            //_img.sprite = Resources.Load<Sprite>("buildings/factory/");

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_img.sprite is null) return;
            _img.color = _diceSelected ? Color.white : Color.grey;
            _diceSelected = !_diceSelected;
            //UIController.Instance.SelectDice(_diceSelected, _diceValue);
        }

        public void RemoveDice()
        {
            _img.gameObject.SetActive(false);
            _diceSelected = false;
            _img.sprite = null;
        }

        public void ShowDiceValue(bool show)
        {
            _text.gameObject.SetActive(show);
        }

        public void ToggleShowDiceValue()
        {
            _text.gameObject.SetActive(!_text.gameObject.activeSelf);
        }
    }
}
