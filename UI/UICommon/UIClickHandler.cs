using Game.Expansion;
using UI.UICommon;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickHandler : MonoBehaviour, IPointerClickHandler
{
    #region Fields

    private IClickHandler _callback;
    private DetectorDoubleClick _detectorDoubleClick = new();
    private UICarrierItem _carrierItem;

    #endregion

    private void Start()
    {
        _callback = GetComponent<IClickHandler>();
        _carrierItem = UICarrierItem.Instance;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_carrierItem.IsEmpty())
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _callback.OnClickWithLeftShift(eventData);
                }
                else
                {
                    _detectorDoubleClick.clickNum = eventData.clickCount;
                    if (eventData.clickCount == 1 && _detectorDoubleClick.isTimeCheckAllowed)
                    {
                        _detectorDoubleClick.firstClickTime = Time.realtimeSinceStartup;
                        StartCoroutine(_detectorDoubleClick.IsDoubleClick((isDoubleClick) =>
                        {
                            if (isDoubleClick)
                                _callback.OnDoubleClick(eventData);
                            else
                                _callback.OnOneClick(eventData);
                        }));
                    }
                }
            }
            else
                _callback.OnOneClick(eventData);
        }
    }
}
