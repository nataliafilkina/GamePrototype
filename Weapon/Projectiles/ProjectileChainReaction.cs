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
        [SerializeField]
        private Transform _detectionPoint;

        private List<Transform> _beatenEnemies = new();
        private Transform _currentEnemy;
        private Collider2D _nextEnemyCollider;

        private void Update()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(_detectionPoint.position, transform.up, _distanceRayCheckCollisions, _solid);
            
            if (hitInfo.collider != null && !hitInfo.collider.CompareTag("Player"))
            {
                if (hitInfo.collider.TryGetComponent(out Enemy enemy))
                {
                    if (_beatenEnemies.Contains(enemy.transform) == false)
                        DetectedEnemy(enemy);
                }
                else
                    OnBurst();
            }

            if (_countOfJumps >= 0)
            {
                if (_nextEnemyCollider != null)
                {
                    Vector3 targetPoint = _nextEnemyCollider.ClosestPoint(transform.position);
                    transform.position = Vector2.MoveTowards(transform.position, targetPoint, _speed * Time.deltaTime);
                    transform.right = targetPoint - transform.position;
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

            _nextEnemyCollider = FindNextTarget()?.GetComponent<Collider2D>();

            if (_nextEnemyCollider == null || _differenceDamage <= 0)
                OnBurst();

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
