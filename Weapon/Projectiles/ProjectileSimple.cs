using UnityEngine;

namespace Weapon
{
    public class ProjectileSimple : Projectile
    {
        private void Update()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, _distanceRayCheckCollisions, _solid);

            if (hitInfo.collider != null && !hitInfo.collider.CompareTag("Player"))
            {
                Collider2D[] attackedColliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, _solid);

                foreach (var collider in attackedColliders)
                {
                    if (collider.TryGetComponent<Enemy>(out var enemy))
                        DetectedEnemy(enemy);
                }

                OnBurst();
            }

            transform.Translate(_speed * Time.deltaTime * Vector2.right);
        }
    }
}
