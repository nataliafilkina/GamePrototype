using GameSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIPopup
{
    public class UISettingsMenu : MonoBehaviour
    {
        #region Fields

        [Inject]
        private Application _application;
        private GameSettingsPresenter _gameSettingsPresenter;

        [SerializeField]
        private Toggle _damageIndicatorToggle;

        [SerializeField]
        private Toggle _fullScreenToggle;

        [SerializeField]
        private TMP_Dropdown _resolutionDropdown;

        #endregion

        private void Start()
        {
            _gameSettingsPresenter = _application.GameSettings;

            var currentSettings = _gameSettingsPresenter.GetSettings();

            _damageIndicatorToggle.isOn = currentSettings.IsDamageIndicatorOn;
            _fullScreenToggle.isOn = currentSettings.IsFullScreen;

            InitResolutionDropdown();

            _damageIndicatorToggle.onValueChanged.AddListener(delegate { ToggleDamageIndicatorChanged(); });
            _fullScreenToggle.onValueChanged.AddListener(delegate { ToggleFullScreenChanged(); });
            _resolutionDropdown.onValueChanged.AddListener(delegate { DropdownResolutionChange(); });
            
        }

        private void InitResolutionDropdown()
        {
            List<string> dropdownOptions = new();
            var resolutions = _gameSettingsPresenter.Resolutions;

            Resolution currentResolution = Screen.currentResolution;
            int dropdownValue = 0;

            int index = 0;
            foreach (Resolution resolution in resolutions)
            {
                dropdownOptions.Add(resolution.width + " x " + resolution.height);

                if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
                    dropdownValue = index;

                index++;
            }

            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(dropdownOptions);
            _resolutionDropdown.value = dropdownValue;
            _resolutionDropdown.RefreshShownValue();
        }

        private void ToggleDamageIndicatorChanged()
        {
            _gameSettingsPresenter.OnDamageIndicatorChange(_damageIndicatorToggle.isOn);
        }

        private void ToggleFullScreenChanged()
        {
            _gameSettingsPresenter.OnFullScreenChange(_fullScreenToggle.isOn);
        }

        private void DropdownResolutionChange()
        {
            _gameSettingsPresenter.OnResolutionChange(_resolutionDropdown.value);
        }
    }
}
