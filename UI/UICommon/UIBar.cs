#if UNITY_EDITOR
using InspectorDraw;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.UICommon
{
    public class UIBar : MonoBehaviour
    {
        [SerializeField]
        private bool _isVisiableValue;

#if UNITY_EDITOR
        [DrawIf("_isVisiableValue", true)]
#endif
        [SerializeField]
        private bool _displayMax;

        private Slider _slider;
        private TMP_Text _valueText;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _valueText = GetComponentInChildren<TMP_Text>();
            _valueText.gameObject.SetActive(_isVisiableValue);
        }

        public void UpdateBar(float currentValue, float maxValue)
        {
            _slider.value = maxValue < int.MaxValue  ? 
                            currentValue / maxValue  : 
                            1;

            if (_isVisiableValue)
            {
                var text = currentValue.ToString();

                if(_displayMax && maxValue < int.MaxValue)
                    text += " / " + maxValue;

                _valueText.SetText(text);
            }
        }
    }
}
