using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;

    private TabGroup _tabGroup;
    public Image background { get; set; }

    private void Awake()
    {
        background = GetComponent<Image>();
    }

    private void Start()
    {
        _tabGroup = GetComponentInParent<TabGroup>();
        _tabGroup.Subscribe(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tabGroup.OnTabExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    public void Select()
    {
        onTabSelected?.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected?.Invoke();
    }
}
