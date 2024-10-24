using Expansion;
using StatusEffectSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public abstract class Projectile : MonoBehaviour
    {
        [SerializeField]
        protected float _lifeTime;
        [SerializeField]
        protected float _distanceRayCheckCollisions;
        [SerializeField]
        protected GameObject _burst;

        protected IForceWeaponInfo _parent;
        protected float _speed;
        protected float _damage;
        protected IReadOnlyCollection<ProbabilityEffect> _effectsData;
        protected float _attackRange;

        protected LayerMask _solid;
        protected LayerMask _enemy;
        protected string[] _hitLayers;
        protected Coroutine _destroyCoroutine;

        private void Start()
        {
            _solid = LayerMask.GetMask("Solid");
            _enemy = LayerMask.GetMask("Enemy");
            _hitLayers = new string[] { "Solid", "Enemy"};
            _destroyCoroutine =  StartCoroutine(Coroutines.DoAfter(_lifeTime, () => Destroy(gameObject)));
        }

        public virtual void OnCreate(IForceWeaponInfo weaponInfo)
        {
            _parent = weaponInfo;
            _speed = weaponInfo.ProjectileSpeed;
            _damage = weaponInfo.Damage;
            _effectsData = weaponInfo.Effects;
            _attackRange = weaponInfo.AttackRange;
        }

        protected virtual void OnDestroy()
        {
            if (_burst != null)
                Instantiate(_burst, transform.position, transform.rotation);
        }

        protected virtual void DetectedEnemy(Enemy enemy)
        {
            if (IsNoObstacleTo(enemy.transform))
            {
                enemy.TakeDamage(_parent, _damage);

                if(enemy.TryGetComponent<EffectableEntity>(out var effectable))
                {
                    foreach (var effectData in _effectsData)
                    {
                        //Value - probability of effect triggering
                        if (effectData.probability >= Random.Range(1, 101))
                        {
                            var effect = effectData.effect.InitializeEffect(_parent, effectable.gameObject);
                            effectable.AddEffect(effect);
                        }
                    }
                }
            }
        }

        protected virtual bool IsNoObstacleTo(Transform endPoint)
        {
            RaycastHit2D hitInfo = Physics2D.Linecast(transform.position, endPoint.position, _solid);

            return hitInfo.collider == null;
        }
    }
}
