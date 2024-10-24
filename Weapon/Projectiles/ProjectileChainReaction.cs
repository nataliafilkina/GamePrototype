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
        private Transform _nextTarget;

        private void Update()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(_detectionPoint.position, transform.right, _distanceRayCheckCollisions, LayerMask.GetMask(_hitLayers));
            
            if(hitInfo.collider != null)
            {
                if (hitInfo.collider.TryGetComponent(out Enemy enemy))
                {
                    if (!_beatenEnemies.Contains(enemy.transform))
                    {
                        DetectedEnemy(enemy);
                        StopCoroutine(_destroyCoroutine);
                        Jump(enemy);
                    }
                }
                else
                    Destroy(gameObject);
            }
            
            if (_nextTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, _nextTarget.position, _speed * Time.deltaTime);
                transform.right = _nextTarget.position - transform.position;
            }
            else
                transform.Translate(_speed * Time.deltaTime * Vector2.right);
        }

        private void Jump(Enemy lastEnemy)
        {
            _countOfJumps--;
            _damage -= _differenceDamage;

            if (_countOfJumps > 0 && _damage > 0)
            {
                _beatenEnemies.Add(lastEnemy.transform);
                _nextTarget = FindNextTarget();

                if (_nextTarget == null)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }

        private Transform FindNextTarget()
        {
            Collider2D[] attackedColliders = Physics2D.OverlapCircleAll(transform.position, _jumpRadius, _enemy);
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
                    !_beatenEnemies.Contains(enemy.transform)
                    && IsNoObstacleTo(enemy.transform))
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(transform.position, _jumpRadius);
        }
    }
}
