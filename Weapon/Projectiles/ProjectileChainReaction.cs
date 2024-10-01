using Expansion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapon
{ 
    public class ProjectileChainReaction : Projectile
    {
        [SerializeField]
        private int _countOfJumps;
        [SerializeField]
        private float _jumpRadius;
        [SerializeField]
        private float _differenceDamage;

        private List<Transform> _beatenEnemies = new();
        private Transform _currentEnemy;
        private Transform _nextEnemy;

        private void Update()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, _distanceRayCheckCollisions, _solid);
            
            if (hitInfo.collider != null && !hitInfo.collider.CompareTag("Player"))
            {
                if (hitInfo.collider.TryGetComponent(out Enemy enemy))
                {
                    if(_nextEnemy == null || !_beatenEnemies.Contains(enemy.transform))
                        DetectedEnemy(enemy);
                }
                else
                    OnBurst();
            }

            if (_countOfJumps >= 0)
            {
                if (_nextEnemy != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, _nextEnemy.position, _speed * Time.deltaTime);
                    //transform.rotation = Quaternion.FromToRotation(transform.position, _nextEnemy.position - transform.position);
                    transform.right = _nextEnemy.position - transform.position;
                }
                else
                    transform.Translate(_speed * Time.deltaTime * Vector2.right);
            }
            else
                OnBurst();
        }

        protected override void DetectedEnemy(Enemy enemy)
        {
            base.DetectedEnemy(enemy);

            _countOfJumps--;
            _damage -= _differenceDamage;
            _beatenEnemies.Add(enemy.transform);

            _nextEnemy = FindNextTarget();

            if (_nextEnemy == null || _differenceDamage <= 0)
                Destroy(gameObject);

            _currentEnemy = enemy.transform;
            StopCoroutine(_destroyCoroutine);
            _destroyCoroutine = StartCoroutine(Coroutines.DoAfter(_lifeTime, () => Destroy(gameObject)));

        }

        private Transform FindNextTarget()
        {
            Collider2D[] attackedColliders = Physics2D.OverlapCircleAll(transform.position, _jumpRadius, _solid);
            var enemies = TakeEnemies(attackedColliders);
            var orderEnemies = OrderByDistance(enemies, _countOfJumps + 1);

            return orderEnemies.Count > 0 ? orderEnemies[0] : null;
        }

        private List<Transform> TakeEnemies(Collider2D[] colliders)
        {
            List<Transform> transforms = new();

            foreach (var collider in colliders)
            {
                if(collider.TryGetComponent<Enemy>(out var enemy) && 
                    enemy.transform != _currentEnemy &&
                    !_beatenEnemies.Contains(enemy.transform))
                    transforms.Add(enemy.transform);
            }

            return transforms;
        }

        private List<Transform> OrderByDistance(List<Transform> transforms, int lengthOrder = int.MaxValue)
        {
            var currentPosition = transform.position;
            return transforms.OrderBy(variable => (variable.position - currentPosition).sqrMagnitude).
                           Take(Mathf.Min(lengthOrder, transforms.Count())).
                           ToList();
        }
    }
}
