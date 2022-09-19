using UnityEngine;

namespace ScriptableObjects
{
    public interface IProjectileConfig
    {
        public float speed { get; }

        public GameObject impactEffect { get; }
    }

    [CreateAssetMenu(fileName = "new projectile config", menuName = "Game/Projectile Configuration", order = 0)]
    public class ProjectileConfiguration : ScriptableObject, IProjectileConfig
    {
        public float projectileSpeed = 1.0f;
        public GameObject impactEffectPrefab;

        public float speed => projectileSpeed;

        public GameObject impactEffect => impactEffectPrefab;
    }
}