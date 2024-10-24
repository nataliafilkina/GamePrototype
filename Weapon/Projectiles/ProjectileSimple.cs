using UnityEngine;

namespace Weapon
{
    public class ProjectileSimple : Projectile
    {
        private void Update()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, _distanceRayCheckCollisions, LayerMask.GetMask(_hitLayers));

            if (hitInfo.collider != null)
            {
                Collider2D[] attackedColliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, _enemy);

                foreach (var collider in attackedColliders)
                {
                    if (collider.TryGetComponent<Enemy>(out var enemy))
                        DetectedEnemy(enemy);
                }

                Destroy(gameObject);
            }

            transform.Translate(_speed * Time.deltaTime * Vector2.right);
        }
    }
}
