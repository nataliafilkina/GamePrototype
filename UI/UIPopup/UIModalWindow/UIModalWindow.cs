using System;
using TMPro;
using UI.UIModalWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ModalWindow
{
    public class UIModalWindow : MonoBehaviour, IModalWindow
    {
        [Header("Header")]
        [SerializeField]
        protected TextMeshProUGUI _headerText;

        [Header("Content")]
        [SerializeField]
        protected TextMeshProUGUI _contentText;

        [Header("Buttons")]
        [SerializeField]
        protected Button _confirmButton;
        [SerializeField]
        protected Button _declineButton;

        protected Action _onConfirmAction;
        protected Action _onDeclineAction;

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Update()
        {
            if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
                Confirm();
            if (Input.GetKeyUp(KeyCode.Escape))
                Decline();
        }

        public void Show(string header, string content, Vector3 position,
                        Action confirmAction, Action declineAction = null)
        {
            _headerText.text = header;
            _contentText.text = content;
            gameObject.transform.position = position;

            _onConfirmAction = confirmAction;
            _onDeclineAction = declineAction;
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            _headerText.text = "";
            _contentText.text = "";

            gameObject.SetActive(false);
        }

        public virtual void Confirm()
        {
            _onConfirmAction?.Invoke();
            Close();
        }

        public virtual void Decline()
        {
            _onDeclineAction?.Invoke();
            Close();
        }
    }
}
