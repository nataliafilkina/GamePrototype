using Stats;
using UI.UICommon;

public class HealthBar : UIBar
{
    private Health _health;

    private void Start()
    {
        var ownerCharacterStats = ExpansionOfBasicFunctions.FindComponentInParent<CharacterStats>(gameObject);
        if (ownerCharacterStats != null)
        {
            _health = ownerCharacterStats.GetComponent<CharacterStats>().Health;
            _health.OnHealthChanged += OnHealthChanged;
            UpdateBar(_health.CurrentValue, _health.BaseValue);
        }
    }

    private void OnHealthChanged(object source, float oldHealth, float newHealth) 
    {
        UpdateBar(newHealth, _health.BaseValue);
    }
}
