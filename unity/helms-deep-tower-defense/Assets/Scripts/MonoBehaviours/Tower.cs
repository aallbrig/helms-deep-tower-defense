using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Model.Combat;
using MonoBehaviours.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours
{
    public class Tower : MonoBehaviour, IDamageable, IKillable, ITowerConfig, IAttack, IHaveHealth
    {
        public LayerMask layerMaskFilter;
        public TowerConfiguration config;
        public Transform firePoint;
        [SerializeField] private BehaviorTree behaviorTree;
        private Collider[] _collidersWithinRange;
        private float _currentHealth;
        private float _lastAttackTime;
        private float _lastSenseTime;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<TowerConfiguration>();
            _currentHealth = MaxHealth;
            behaviorTree = config.BuildBehaviorTree(gameObject);
        }

        private void Update() => behaviorTree.Tick();

        private void OnEnable()
        {
            _lastSenseTime = Time.time - config.senseDelay;
            _lastAttackTime = Time.time - config.attackDelay;
            Killed += OnKilled;
        }
        private void OnDisable() => Killed -= OnKilled;

        public event Action<GameObject> AttackedTarget;

        public void Attack(GameObject target)
        {
            var projectile = Instantiate(config.projectilePrefab, firePoint.position, firePoint.rotation);
            var projectileComponent = projectile.GetComponent<Projectile>();
            projectileComponent.DamageableCollided += damageable => damageable.Damage(config.damage);
            projectileComponent.layerMaskFilter = layerMaskFilter;
            AttackedTarget?.Invoke(target);
        }

        public event Action<float> Damaged;

        public void Damage(float damage)
        {
            if (_currentHealth == 0) return;

            _currentHealth -= damage;
            Damaged?.Invoke(damage);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Kill();
            }
        }

        public float CurrentHealthNormalized() => _currentHealth / MaxHealth;

        public event Action Killed;

        public void Kill() => Killed?.Invoke();

        public float MaxHealth => config.MaxHealth;

        public float Range => config.Range;

        private void OnKilled() => gameObject.SetActive(false);
        public TaskStatus SenseForTargets()
        {
            _lastSenseTime = Time.time;
            _collidersWithinRange = Physics.OverlapSphere(transform.position, config.range, layerMaskFilter);
            return TaskStatus.Success;
        }
        public TaskStatus AttackTarget()
        {
            _lastAttackTime = Time.time;
            var nearestTarget = FindNearestTarget(_collidersWithinRange);
            Attack(nearestTarget);
            return TaskStatus.Success;
        }
        private GameObject FindNearestTarget(Collider[] collidersWithinRange)
        {
            var minDistance = config.range + 1f;
            GameObject nearestEnemy = null;
            foreach (var other in collidersWithinRange)
            {
                var distance = Vector3.Distance(other.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = other.gameObject;
                }
            }
            return nearestEnemy;
        }
        public bool HasTarget() => _collidersWithinRange.Length > 0;
        public bool CanSenseTargets() => Time.time - _lastSenseTime >= config.senseDelay;
        public bool CanAttackTarget() => Time.time - _lastAttackTime >= config.attackDelay;
    }
}