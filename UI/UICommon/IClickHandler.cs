using UnityEngine.EventSystems;

namespace UI.UICommon
{
    public interface IClickHandler
    {
        public void OnOneClick(PointerEventData eventData);
        public void OnDoubleClick(PointerEventData eventData);
        public void OnClickWithLeftShift(PointerEventData eventData);
    }
}
