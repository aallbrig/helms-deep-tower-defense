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
        public AttackRange attackRange;
        private float _currentHealth;
        private BehaviorTree _behaviorTree;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<TowerConfiguration>();
            _currentHealth = MaxHealth;
            _behaviorTree = config.BuildBehaviorTree(gameObject);
        }

        private void Start()
        {
            Killed += () => gameObject.SetActive(false);
        }

        private void Update()
        {
            _behaviorTree.Tick();
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

        public event Action Killed;

        public void Kill()
        {
            Killed?.Invoke();
        }

        public float MaxHealth => config.MaxHealth;
        public float Range => config.Range;

        public event Action<GameObject> AttackedTarget;

        public void Attack(GameObject target)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                AttackedTarget?.Invoke(target);
            }
        }

        public float CurrentHealthNormalized() => _currentHealth / MaxHealth;
        public bool HasTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, 3f, layerMaskFilter);
            return colliders.Length > 0;
        }
        public TaskStatus AttackTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, 3f, layerMaskFilter);
            Attack(colliders[0].gameObject);
            return TaskStatus.Success;
        }
    }
}