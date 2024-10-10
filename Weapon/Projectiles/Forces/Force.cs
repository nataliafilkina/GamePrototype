using Expansion;
using UnityEngine;

namespace Weapon
{
    public class Force : MonoBehaviour, IWeapon
    {
        [Header("Characteristics")]
        [SerializeField]
        public ForceWeaponSO _defualtInfo;
        [SerializeField]
        private GameObject _projectile;
        [SerializeField]
        private float _sightOffset = 0;

        [Header("View")]
        [SerializeField]
        private bool _isInHands = true;

        private ForceStats ForceCurrentStats = new ForceStats();
        private Renderer _renderer;
        private Coroutine _rateOfFireCoroutine;

        public IWeaponInfo DefaultStats => _defualtInfo;
        public IWeaponInfo CurrentStats => ForceCurrentStats;

        public bool ReadyShoot { get; private set; } = true;

        protected void OnValidate()
        {
            var sprite = GetComponent<SpriteRenderer>();
            sprite.sortingOrder = _isInHands ? 14 : 20;
        }

        protected void Awake()
        {
            _renderer = GetComponent<Renderer>();
            ForceCurrentStats.Set(_defualtInfo);
        }

        protected void OnEnable()
        {
            OnValidate();
        }

        protected void OnDisable()
        {
            if (_rateOfFireCoroutine != null)
            {
                StopCoroutine(_rateOfFireCoroutine);
                ReadyShoot = true;
                _renderer.enabled = true;
            }
        }

        public void Attack()
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion projectileRotation = Quaternion.Euler(0f, 0f, rotateZ + _sightOffset);

            ReadyShoot = false;
            _renderer.enabled = false;

            Debug.Log(CurrentStats.Damage);

            var projectile = Instantiate(_projectile, transform.position, projectileRotation);
            projectile.GetComponent<Projectile>().OnCreate(ForceCurrentStats);

            _rateOfFireCoroutine = StartCoroutine(Coroutines.DoAfter(CurrentStats.RateOfFire, () =>
            {   
                ReadyShoot = true; 
                _renderer.enabled = true;
            }));
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red - new Color(0, 0, 0, 0.5f);
            Gizmos.DrawSphere(transform.position, CurrentStats.AttackRange);
        }
#endif
    }
}
