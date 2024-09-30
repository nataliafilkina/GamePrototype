using GameSystems;
using MessageSystem;
using Stats;
using UICommon.Colors;
using UnityEngine;
using Zenject;

public class DamageIndicatorSpawner : MessageSpawner
{
    [Inject]
    private Application _application;
    private GameSettingsPresenter _gameSettingsData;

    [SerializeField]
    private IndicatorColors _colors;

    private Health _health;

    private void Start()
    {
        _gameSettingsData = _application.GameSettings;
        _health = gameObject.GetComponentInParent<CharacterStats>().Health;

        gameObject.SetActive(false);
        gameObject.SetActive(_gameSettingsData.GetSettings().IsDamageIndicatorOn);
        _gameSettingsData.OnDamageIndicatorChanged += IsOn => gameObject.SetActive(IsOn);
    }

    private void OnEnable()
    {
        if (_health != null)
            _health.OnHealthChanged += SpawnMessage;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnHealthChanged -= SpawnMessage;
    }

    private void SpawnMessage(IChangingStat source, float oldHealth, float newHealth)
    {
        var damage = oldHealth - newHealth;
        var message = new MessageData(damage.ToString())
        {
            FaceColor = _colors.GetColorBySchoolOfMagic(source.SchoolOfMagic)
        };

        SpawnMessage(message);
    }
}
