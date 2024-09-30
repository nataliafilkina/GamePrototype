using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusEffectSystem
{
    public class EffectableEntity : MonoBehaviour
    {
        [field: SerializeField]
        public Material DefaultMaterial {  get; private set; }

        private readonly Dictionary<ScriptableEffect, StatusEffect> _effects = new();
        private List<ScriptableEffect> _bannedEffects = new();

        private void Start()
        {
            if(DefaultMaterial == null)
            {
                var renderer = GetComponentInChildren<Renderer>();
                DefaultMaterial = renderer.material;
            }
        }

        private void Update()
        {
            foreach(var effect in _effects.Values.ToList()) 
            {
                effect.Tick(Time.deltaTime);

                if(effect.IsFinished)
                {
                    RemoveEffect(effect);
                }
            }
        }

        public void AddEffect(StatusEffect effect)
        {
            var effectData = effect.EffectData;

            if(_effects.ContainsKey(effectData)) 
            {
                if(effectData.IsUpdate)
                {
                    RemoveEffect(effect);
                    _effects.Add(effectData, effect);
                    AddBannedEffects(effectData.EffectsNeedCancel);
                }

                _effects[effectData].Activate();
            }
            else
            {
                if (!_bannedEffects.Contains(effectData))
                {
                    _effects.Add(effectData, effect);
                    AddBannedEffects(effectData.EffectsNeedCancel);
                    effect.Activate();
                }
            }
        }

        private void RemoveEffect(StatusEffect effect)
        {
            var effectData = effect.EffectData;

            if (_effects.ContainsKey(effectData))
            {
                if (!effect.IsFinished)
                    effect.End();

                RemoveBannedEffects(effectData.EffectsNeedCancel);
                _effects.Remove(effectData);
            }
        }

        private void AddBannedEffects(IReadOnlyList<ScriptableEffect> effects)
        {
            foreach (var effect in effects)
            {
                _bannedEffects.Add(effect);

                if (_effects.TryGetValue(effect, out var statusEffect))
                {
                    RemoveEffect(statusEffect);
                }
            }
        }

        private void RemoveBannedEffects(IReadOnlyList<ScriptableEffect> effects)
        {
            foreach (var effect in effects)
                _bannedEffects.Remove(effect);
        }

        public bool HasEffectByType(Type effectType, out ScriptableEffect effectData)
        {
            var effect = _effects.FirstOrDefault(pair => pair.Key.GetType().Equals(effectType));
            var hasEffect = !effect.Equals(default(KeyValuePair<ScriptableEffect, StatusEffect>));

            effectData = hasEffect ? effect.Key : null;

            return hasEffect;
        }
    }
}