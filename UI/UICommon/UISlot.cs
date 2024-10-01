using UI.UICommon;
using UICommon.Colors;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(UIClickHandler))]
public class UISlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IClickHandler
{
    #region Fields

    [SerializeField]
    private Sprite _idleIcon;

    [SerializeField]
    private Sprite _pressedIcon;

    [SerializeField]
    private Sprite _hoverIcon;

    [SerializeField]
    private bool IsPressingSave;

    private Image _image;
    private Color32 _notAvailable = CommonColors.NotAvailable;
    private Color32 _available = CommonColors.Available;

    public bool IsPressed { get; private set; } = false;
    public bool IsAvailable { get; private set; } = true;

    protected UIItemViewInSlot _uiItem;

    [Inject]
    protected UICarrierItem _carrierItem { get; private set; }

    #endregion

    protected virtual void Awake()
    {
        _uiItem = GetComponentInChildren<UIItemViewInSlot>();
        _image = GetComponentInChildren<Image>();
        _image.sprite = _idleIcon;
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
        IsPressed = false;
        _image.sprite = _idleIcon;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(IsAvailable && !IsPressed)
            _image.sprite = _hoverIcon;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(IsAvailable && !IsPressed)
            _image.sprite = _idleIcon;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (IsAvailable)
        {
            _image.sprite = (IsPressingSave && IsPressed) ? _idleIcon : _pressedIcon;

            if (IsPressingSave)
                IsPressed = !IsPressed;
        }
    }

    public virtual void OnOneClick(PointerEventData eventData)
    {
        OnPointerClick(eventData);
    }

    public virtual void OnDoubleClick(PointerEventData eventData)
    {
    }

    public virtual void OnClickWithLeftShift(PointerEventData eventData)
    {
    }

    public virtual void SetAvailable(bool available)
    {
        _image.color = available ? _available : _notAvailable; 
        IsAvailable = available; 
    }

    public virtual void Refresh() { }
}
