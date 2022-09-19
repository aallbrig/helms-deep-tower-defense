using System;
using Model.Combat;
using UnityEngine;

namespace MonoBehaviours.Combat
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 3.0f;
        public LayerMask layerMaskFilter;
        public GameObject impactEffect;
        private Rigidbody _rigidbody;
        private void Awake() => _rigidbody = GetComponent<Rigidbody>();
        private void Start() => _rigidbody.velocity = transform.forward * speed;
        private void OnBecameInvisible() => Destroy(gameObject);
        private void OnTriggerEnter(Collider other)
        {
            if (IsInLayerMask(other.gameObject, layerMaskFilter))
            {
                if (impactEffect) Instantiate(impactEffect);
                Destroy(gameObject);
                if (other.TryGetComponent<IDamageable>(out var damageable))
                    DamageableCollided?.Invoke(damageable);
            }
        }

        public event Action<IDamageable> DamageableCollided;

        private bool IsInLayerMask(GameObject other, LayerMask layerMask) => layerMask == (layerMask | (1 << other.layer));
    }
}