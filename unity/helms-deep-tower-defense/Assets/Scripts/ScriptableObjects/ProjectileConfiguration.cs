using UnityEngine;

namespace ScriptableObjects
{
    public interface IProjectileConfig
    {
        public float speed { get; }

        public GameObject impactEffect { get; }

        public LayerMask layerMaskFilter { get; }
    }

    [CreateAssetMenu(fileName = "new projectile config", menuName = "Game/Projectile Configuration", order = 0)]
    public class ProjectileConfiguration : ScriptableObject, IProjectileConfig
    {
        public float projectileSpeed = 1.0f;
        public GameObject impactEffectPrefab;
        public LayerMask layerMask;

        public float speed => projectileSpeed;

        public GameObject impactEffect => impactEffectPrefab;

        public LayerMask layerMaskFilter => layerMask;
    }
}