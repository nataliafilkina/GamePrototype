using System;
using UnityEngine;

namespace UI.UIModalWindow
{
    public interface IModalWindow
    {
        public void Show(string header, string content, Vector3 position, Action confirmAction, Action declineAction = null);
        public void Close();
        public void Confirm();
        public void Decline();
    }
}
