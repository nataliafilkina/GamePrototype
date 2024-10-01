using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemViewInSlot : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Sprite _defaultIcon;

    private Image imageIcon;
    private TextMeshProUGUI textAmount;

    #endregion

    private void Awake()
    {
        if (imageIcon == null)
            imageIcon = GetComponentInChildren<Image>(true);

        if (textAmount == null)
            textAmount = GetComponentInChildren<TextMeshProUGUI>(true);

        imageIcon.raycastTarget = false;

        if(_defaultIcon != null)
            SetView(_defaultIcon, 0);
    }

    public void SetView(Sprite icon, int amount)
    {
        gameObject.SetActive(true);
        imageIcon.sprite = icon;
        imageIcon.gameObject.SetActive(true);

        SetAmount(amount);
    }

    public void SetAmount(int amount) 
    {
        bool textAmountEnabled = amount > 1;
        textAmount.gameObject.SetActive(textAmountEnabled);

        if (textAmountEnabled)
            textAmount.text = $"x{amount}";
    }

    public void Clear()
    {
        if (_defaultIcon == null)
            gameObject.SetActive(false);
        else
            SetView(_defaultIcon, 0);
    }
}

