using System;
using Model.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Combat
{
    public class Projectile : MonoBehaviour
    {
        public ProjectileConfiguration config;
        private Rigidbody _rigidbody;

        public IProjectileConfig Config => config;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();
        private void Start()
        {
            config ??= ScriptableObject.CreateInstance<ProjectileConfiguration>();
            _rigidbody.velocity = transform.forward * Config.speed;
        }
        private void OnBecameInvisible() => Destroy(gameObject);
        private void OnTriggerEnter(Collider other)
        {
            if (IsInLayerMask(other.gameObject, Config.layerMaskFilter))
            {
                if (Config.impactEffect) Instantiate(Config.impactEffect);
                Destroy(gameObject);
                if (other.TryGetComponent<IDamageable>(out var damageable))
                    DamageableCollided?.Invoke(damageable);
            }
        }

        public event Action<IDamageable> DamageableCollided;

        private static bool IsInLayerMask(GameObject other, LayerMask layerMask) =>
            layerMask == (layerMask | (1 << other.layer));
    }
}