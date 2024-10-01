using Upgrade;
using TMPro;
using UI.UICommon;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIManagerHUD : MonoBehaviour
    {
        [SerializeField]
        private UIBar _experienceBar;

        [SerializeField]
        private TMP_Text _levelText;

        [Inject]
        private Experience _experience;

        private void Start()
        {
            _experience.OnChanged += OnChangedExperience;
            _experience.OnLevelUp += UpdateLevelUp;

            _experienceBar.UpdateBar(_experience.CurrentExperience, _experience.ExperienceToNextLevel);
            UpdateLevelUp(_experience.CurrentLevel);
        }

        private void OnChangedExperience(int oldValue, int newValue)
        {
            _experienceBar.UpdateBar(newValue, _experience.ExperienceToNextLevel);
        }

        private void UpdateLevelUp(LevelSO level)
        {
            _levelText.text = level.Number.ToString();
        }
    }
}
