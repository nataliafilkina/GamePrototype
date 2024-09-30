using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusEffectSystem
{
    public abstract class StatusEffect
    {
        #region Fields

        public ScriptableEffect EffectData { get; private set; }
        public bool IsFinished;

        protected float _tickRate = 1f;
        protected float _duration;
        protected int _countStacks;

        protected readonly GameObject _target;
        protected readonly IChangingStat _source;

        private float _timeSinceLastTick;

        protected readonly List<Renderer> _renderers = new();
        protected readonly Material _defaultMaterial;

        #endregion

        public StatusEffect(IChangingStat source, ScriptableEffect effectData, GameObject target)
        {
            _target = target;
            EffectData = effectData;
            _tickRate = effectData.Period;
            _source = source;

            if (EffectData.Material != null)
                _renderers = TryGetRenderers();

            _defaultMaterial = target.GetComponent<EffectableEntity>().DefaultMaterial;
        }

        public void Tick(float delta)
        {
            _duration -= delta;
            _timeSinceLastTick += delta;

            if(_timeSinceLastTick >= _tickRate)
            {
                ApplyTick();
                _timeSinceLastTick = 0f;
            }

            if(_duration <= 0)
            {
                End();
                IsFinished = true;
            }
        }

        public void Activate()
        {
            if(EffectData.IsEffectStacked || _duration <= 0)
            {
                ApplyEffect();
                _countStacks++;
            }

            if(EffectData.IsDurationStacked || _duration <= 0) 
            {
                _duration += EffectData.Duration;
            }
        }

        protected virtual void ApplyEffect()
        {
            foreach (var renderer in _renderers)
            {
                renderer.material = EffectData.Material;
            }
        }

        public virtual void End()
        {
            foreach (var renderer in _renderers)
            {
                renderer.material = _defaultMaterial;
            }
        }


        protected virtual void  ApplyTick()
        {
            if (EffectData.IsPeriodic)
            {
                ApplyEffect();
            }
        }

        protected List<Renderer> TryGetRenderers()
        {
            List<Renderer> _renderers = _target.GetComponentsInChildren<Renderer>().ToList();

            if (_renderers.Count == 0)
                _renderers = _target.GetComponents<Renderer>().ToList();

            return _renderers;
        }
    }
}
