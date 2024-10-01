using System;
using TMPro;
using UnityEngine;

namespace UI.ModalWindow
{
    public class UIInputWindow : UIModalWindow
    {
        [Header("Content")]
        [SerializeField]
        private TMP_InputField _inputField;
        private int _maxValue;
        private int _minValue;

        private new Action<string> _onConfirmAction;

        public void Show(string header, string content, Vector3 position, TMP_InputField.ContentType contentType, int characterLimit, string defaultValueField, int minValue, int maxValue, 
                            Action<string> confirmAction, Action declineAction = null)
        {
            _inputField.contentType = contentType;
            _inputField.characterLimit = characterLimit;
            _inputField.text = defaultValueField;
            _minValue= minValue;
            _maxValue = maxValue;
            _onConfirmAction = confirmAction;

            Show(header, content, position, null, declineAction);
        }

        public override void Close()
        {
            _inputField.text = "";
            base.Close();
        }

        public void Increase()
        {
            var currentValue = int.Parse(_inputField.text);
            if(currentValue < _maxValue)
                _inputField.text = (++currentValue).ToString();
        }

        public void Decrease()
        {
            var currentValue = int.Parse(_inputField.text);
            if (currentValue > _minValue)
                _inputField.text = (--currentValue).ToString();
        }

        public override void Confirm()
        {
            if (ValidateInput())
            {
                _onConfirmAction?.Invoke(_inputField.text);
                Close();
            }
            else
                Debug.Log("Введено некорректное значение");
        }

        public bool ValidateInput()
        {
            return !(_maxValue != -1 && (int.Parse(_inputField.text) > _maxValue) || int.Parse(_inputField.text) < _minValue);
        }
    }
}
