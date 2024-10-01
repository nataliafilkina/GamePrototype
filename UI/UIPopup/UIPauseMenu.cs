using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [Header("UIControls")]
    [SerializeField]
    private Button _continueButton;
    [SerializeField]
    private Button _settingsButton;

    [Header("Other")]
    [SerializeField]
    private GameObject _settingsPanel;
    private UIPopupManager _popupManager;

    private void Awake()
    {
        _popupManager = GetComponentInParent<UIPopupManager>();

        _settingsButton?.onClick.AddListener(delegate { OnSettingsClick(); });
        _continueButton?.onClick.AddListener(delegate { OnContinueClick(); });
    }

    private void OnSettingsClick()
    {
        _popupManager.OpenWindow(_settingsPanel);
    }

    private void OnContinueClick()
    {
        _popupManager.CloseTopWindow();
    }
}
