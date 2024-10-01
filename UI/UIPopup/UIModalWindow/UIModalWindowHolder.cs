using UI.UIModalWindow;
using UnityEngine;

namespace UI.ModalWindow
{
    public class UIModalWindowHolder : MonoBehaviour
    {
        public IModalWindow ConfirmWindow { get; private set; }
        public IModalWindow InputWindow { get; private set; }

        private void Start()
        {
            ConfirmWindow = GetComponentInChildren<UIModalWindow>(true);
            InputWindow = GetComponentInChildren<UIInputWindow>(true);
        }
    }
}
