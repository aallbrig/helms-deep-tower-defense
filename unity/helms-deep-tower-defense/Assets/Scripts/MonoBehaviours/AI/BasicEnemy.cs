using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Model.Combat;
using Model.Locomotion;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.AI
{
    public class BasicEnemy : MonoBehaviour, IFollowPath, IDamageable, IHaveHealth, IKillable
    {

        public EnemyConfiguration config;
        public Path currentPath;
        public float maxHealth = 3f;
        public Transform theTarget;
        public bool debugEnabled;
        private Transform _attackPoint;

        private BehaviorTree _behaviorTree;
        private float _currentHealth;
        private int _currentPathPointIndex;
        private IDamageable _damageable;
        private float _lastAttackTime;

        public Transform Target
        {
            get => theTarget;
            set
            {
                if (theTarget != value)
                {
                    DebugLog("Target | setting target");
                    theTarget = value;
                    NewTargetAcquired?.Invoke(theTarget);
                }
            }
        }

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<EnemyConfiguration>();
            _behaviorTree = config.BuildBehaviorTree(gameObject);
            _lastAttackTime = Time.time - config.AttackDelay;
            path = currentPath;
            NewTargetAcquired += newTarget => transform.LookAt(newTarget);
            _currentHealth = maxHealth;
        }

        private void Update() => _behaviorTree.Tick();

        private void OnTriggerEnter(Collider other)
        {
            if (_damageable != null) return;

            if (_damageable == null && other.TryGetComponent<IDamageable>(out var damageable))
            {
                _damageable = damageable;
                DiscoveredDamageable?.Invoke(_damageable);
            }
            if (_attackPoint == null && other.TryGetComponent<IAssignAttackPoints>(out var attackPointAssigner))
            {
                var attackPoint = attackPointAssigner.AssignAttackPoint();
                _attackPoint = attackPoint;
                ForgetPath();
                ForgetTarget();
                Target = _attackPoint;
                AttackPointAcquired?.Invoke(_attackPoint);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_damageable == null && _attackPoint == null) return;

            if (other.TryGetComponent<IDamageable>(out var damageable))
                if (_damageable == damageable)
                {
                    _damageable = null;
                    ForgotDamageable?.Invoke(damageable);
                }
        }

        public event Action<float> Damaged;

        public void Damage(float damage)
        {
            _currentHealth -= damage;
            Damaged?.Invoke(damage);
            if (_currentHealth <= 0) Kill();
        }

        public Path path
        {
            get => currentPath;
            set => currentPath = value;
        }

        public float CurrentHealthNormalized() => _currentHealth / maxHealth;

        public event Action Killed;

        public void Kill()
        {
            Killed?.Invoke();
            Destroy(gameObject);
        }

        public event Action<Transform> NewTargetAcquired;

        public event Action<Vector3> MovedTowardsPosition;

        public event Action<GameObject> ReachedPoint;

        public event Action<IDamageable> ForgotDamageable;

        public event Action<IDamageable> DiscoveredDamageable;

        public event Action<Transform> AttackPointAcquired;

        public event Action<IDamageable> DamageableAttacked;

        public event Action<Path> ForgotPath;

        public event Action<Transform> ForgotTarget;

        public TaskStatus MoveToTarget()
        {
            DebugLog($"<BasicEnemy> | MoveToTarget() | (Target {Target}, reached target? {ReachedTarget()})");
            if (Target == null) return TaskStatus.Failure;

            if (ReachedTarget()) return TaskStatus.Success;

            transform.position = Vector3.MoveTowards(
                transform.position,
                Target.position,
                config.MoveSpeed * Time.deltaTime
            );
            MovedTowardsPosition?.Invoke(Target.position);
            return TaskStatus.Continue;
        }

        private bool ReachedTarget()
        {
            DebugLog($"ReachedTarget | (distance {Vector3.Distance(Target.position, transform.position)})");
            return Vector3.Distance(Target.position, transform.position) < 0.3f;
        }

        public bool HasTarget()
        {
            DebugLog($"HasTarget | (Target != null {Target != null})");
            return Target != null;
        }

        private void ForgetTarget()
        {
            ForgotTarget?.Invoke(Target);
            Target = null;
        }

        public bool HasPath()
        {
            DebugLog($"HasPath | knows about path? {path != null}");
            return path != null;
        }

        public TaskStatus ForgetPath()
        {
            if (path == null) return TaskStatus.Failure;
            ForgotPath?.Invoke(path);
            path = null;
            return TaskStatus.Success;
        }

        public TaskStatus FollowPath()
        {
            if (path == null || path.pathPoints.Count == 0)
            {
                DebugLog("FollowPath | path is null or path points == 0; failure");
                return TaskStatus.Failure;
            }
            DebugLog($"FollowPath | (# of path points {path.pathPoints.Count}, currentPointIndex {_currentPathPointIndex})");

            if (_currentPathPointIndex == path.pathPoints.Count)
            {
                DebugLog("FollowPath | successfully followed path");
                // reset internal state
                _currentPathPointIndex = 0;
                return TaskStatus.Success;
            }

            var currentPoint = path.pathPoints[_currentPathPointIndex];
            DebugLog($"FollowPath | current point location {currentPoint.position}");
            if (Target == null || !Target.Equals(currentPoint))
            {
                var oldPosition = Target == null ? Vector3.zero : Target.position;
                DebugLog($"<BasicEnemy> | FollowPath | setting new target (old: {oldPosition}, new: {currentPoint.position}");
                Target = currentPoint;
            }

            var status = MoveToTarget();
            if (status == TaskStatus.Success)
            {
                ReachedPoint?.Invoke(Target.gameObject);
                _currentPathPointIndex++;
            }

            return status == TaskStatus.Success ? TaskStatus.Continue : status;
        }

        private void DebugLog(string logMessage)
        {
            if (debugEnabled) Debug.Log($"<BasicEnemy> | {logMessage}");
        }

        public bool HasDamageable()
        {
            DebugLog($"HasDamageable | damageable reference exists? {_damageable != null}");
            return _damageable != null;
        }

        public bool CanAttackDamageable()
        {
            DebugLog($"CanAttackDamageable | Can attack? {Time.time - _lastAttackTime >= config.AttackDelay}");
            return Time.time - _lastAttackTime >= config.AttackDelay;
        }

        public TaskStatus AttackDamageable()
        {
            DebugLog("AttackDamageable | Attacking damageable");
            _damageable.Damage(config.AttackDamage);
            DamageableAttacked?.Invoke(_damageable);
            _lastAttackTime = Time.time;
            return TaskStatus.Success;
        }
    }
}