using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    #region Fields

    [Header("Selected tab on enable")]
    [SerializeField]
    private TabButton _defaultButton;
    [SerializeField]
    private bool _isSaveLastChoice;

    [Header("Button background")]
    [SerializeField]
    private Sprite _spriteIdle;
    [SerializeField]
    private Sprite _spriteActive;
    [SerializeField]
    private Sprite _spriteHover;

    [Header("Height button")]
    [SerializeField]
    private float _heightActivButton = 120;
    [SerializeField]
    private float _heightIdleButton = 100;

    private List<TabButton> _tabButtons;
    private TabButton _selectedTab;

    #endregion

    private void OnEnable()
    {
        if(!_isSaveLastChoice)
            OnTabSelected(_defaultButton);
    }

    public void Subscribe(TabButton button)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<TabButton>();
            if (_defaultButton == null)
                _defaultButton = button;
            if (_isSaveLastChoice)
                _selectedTab = button;
        }
        else
            button.background.sprite = _spriteIdle;

        _tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        if(button != _selectedTab)
            button.background.sprite = _spriteHover;
    }

    public void OnTabExit(TabButton button)
    {
        if (button != _selectedTab)
            button.background.sprite = _spriteIdle;
    }

    public void OnTabSelected(TabButton button)
    {
        if (_tabButtons != null && _tabButtons.Count > 0)
        {
            if (_selectedTab != null)
            {
                _selectedTab.background.sprite = _spriteIdle;
                SetButtonHeight(_selectedTab, _heightIdleButton);
                _selectedTab.Deselect();
            }
            _selectedTab = button;
            button.background.sprite = _spriteActive;
            SetButtonHeight(button, _heightActivButton);
            _selectedTab.Select();
            button.transform.SetAsLastSibling();
        }
    }

    private void SetButtonHeight(TabButton button, float height)
    {
        var buttonRectTransform = button.gameObject.GetComponent<RectTransform>();
        buttonRectTransform.sizeDelta = new Vector2(buttonRectTransform.sizeDelta.x, height);
    }

}
