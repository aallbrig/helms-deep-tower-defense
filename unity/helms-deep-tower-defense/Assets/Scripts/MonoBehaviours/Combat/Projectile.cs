using UnityEngine;

namespace MonoBehaviours.Combat
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 3.0f;
        public LayerMask layerMaskFilter;
        private Rigidbody _rigidbody;
        private void Awake() => _rigidbody = GetComponent<Rigidbody>();
        private void Start() => _rigidbody.velocity = transform.forward * speed;
        private void OnBecameInvisible() => Destroy(gameObject);
        private void OnTriggerEnter(Collider other)
        {
            if (!IsInLayerMask(other.gameObject, layerMaskFilter)) Destroy(gameObject);
        }
        private bool IsInLayerMask(GameObject other, LayerMask layerMask) => layerMask.value * (1 << other.layer) > 0;
    }
}