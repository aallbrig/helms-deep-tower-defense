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

        public ITeamConfig TeamConfig { get; set; }

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();
        private void Start()
        {
            config ??= ScriptableObject.CreateInstance<ProjectileConfiguration>();
            if (ReferenceEquals(TeamConfig, null))
            {
                if (TryGetComponent<Team>(out var teamComponent))
                    TeamConfig = teamComponent;
                TeamConfig ??= ScriptableObject.CreateInstance<TeamConfiguration>();
            }
            _rigidbody.velocity = transform.forward * Config.speed;
        }

        private void OnBecameInvisible() => Destroy(gameObject);

        private void OnTriggerEnter(Collider other)
        {
            if (TeamConfiguration.IsInLayerMask(other.gameObject, TeamConfig.enemies))
            {
                if (Config.impactEffect) Instantiate(Config.impactEffect);
                Destroy(gameObject);
                if (other.TryGetComponent<IDamageable>(out var damageable))
                    DamageableCollided?.Invoke(damageable);
            }
        }

        public event Action<IDamageable> DamageableCollided;
    }
}