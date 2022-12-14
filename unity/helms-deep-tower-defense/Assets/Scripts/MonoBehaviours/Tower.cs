using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Model.Combat;
using MonoBehaviours.Combat;
using MonoBehaviours.Commerce;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours
{
    public class Tower : MonoBehaviour, IDamageable, IKillable, ITowerConfig, IAttack, IHaveHealth
    {
        public TowerConfiguration config;
        public Transform firePoint;
        public Transform turret;
        public Animator modelAnimator;
        [SerializeField] private BehaviorTree behaviorTree;
        private Collider[] _collidersWithinRange;
        private float _currentHealth;
        private GameObject _currentTarget;
        private float _lastAttackTime;
        private float _lastSenseTime;
        private ITeamConfig _teamConfig;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<TowerConfiguration>();
            _teamConfig ??= GetComponent<ITeamConfig>();
            _teamConfig ??= ScriptableObject.CreateInstance<TeamConfiguration>();
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
            if (config.shotEffect) Instantiate(config.shotEffect, firePoint.position, firePoint.rotation);
            var projectile = Instantiate(config.projectilePrefab, firePoint.position, firePoint.rotation);
            var projectileComponent = projectile.GetComponent<Projectile>();
            if (modelAnimator) modelAnimator.SetTrigger("Fire");
            projectileComponent.config = config.projectileConfig;
            projectileComponent.TeamConfig = _teamConfig;
            projectileComponent.DamageableCollided += damageable =>
            {
                damageable.GetComponent<IDamageable>().Damage(config.damage);
                if (damageable.TryGetComponent<IKillable>(out var killable) &&
                    damageable.TryGetComponent<IRewardMoney>(out var rewardMoney) &&
                    killable.IsDead)
                    FindObjectOfType<MoneyPurse>().AddReward(rewardMoney);
            };
            AttackedTarget?.Invoke(target);
        }

        public event Action<float> Damaged;

        public void Damage(float damage)
        {
            if (_currentHealth == 0) return;

            _currentHealth -= damage;
            Damaged?.Invoke(damage);

            if (_currentHealth <= 0) Kill();
        }

        public float CurrentHealthNormalized() => _currentHealth / MaxHealth;

        public event Action Killed;

        public bool IsDead => _currentHealth <= 0;

        public void Kill()
        {
            _currentHealth = 0;
            Killed?.Invoke();
        }

        public float MaxHealth => config.MaxHealth;

        public float Range => config.Range;

        private void OnKilled() => gameObject.SetActive(false);
        public TaskStatus SenseForTargets()
        {
            _lastSenseTime = Time.time;
            _collidersWithinRange = Physics.OverlapSphere(transform.position, config.range, _teamConfig.enemies);
            return TaskStatus.Success;
        }
        public TaskStatus AttackTarget()
        {
            _lastAttackTime = Time.time;
            var nearestTarget = FindNearestTarget(_collidersWithinRange);
            Attack(nearestTarget);
            return TaskStatus.Success;
        }
        public TaskStatus FindClosestTarget()
        {
            var closestTarget = FindNearestTarget(_collidersWithinRange);
            if (closestTarget == null) return TaskStatus.Failure;
            _currentTarget = closestTarget;
            return TaskStatus.Success;
        }
        public TaskStatus LookAtClosestTarget()
        {
            if (_currentTarget == null) return TaskStatus.Failure;
            turret.LookAt(_currentTarget.transform);
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
        public bool HasTargets() => _collidersWithinRange.Length > 0;
        public bool CanSenseTargets() => Time.time - _lastSenseTime >= config.senseDelay;
        public bool CanAttackTarget() => Time.time - _lastAttackTime >= config.attackDelay;
    }
}
