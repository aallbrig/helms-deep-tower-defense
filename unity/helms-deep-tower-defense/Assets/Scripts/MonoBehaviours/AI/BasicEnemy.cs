using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Model.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.AI
{
    public class BasicEnemy : MonoBehaviour
    {
        public event Action<Transform> NewTargetAcquired;
        public event Action<Vector3> MovedTowardsPosition;
        public event Action<GameObject> ReachedPoint;
        public event Action<IDamageable> ForgotDamageable;
        public event Action<IDamageable> DiscoveredDamageable;
        public event Action<IDamageable> DamageableAttacked;

        public EnemyConfiguration config;
        public Path path;
        private int _currentPathPointIndex = 0;
        public Transform theTarget;
        public bool debugEnabled = false;
        private IDamageable _damageable;

        public Transform target
        {
            get => theTarget;
            set
            {
                if (theTarget == null || !theTarget.Equals(value))
                {
                    DebugLog("Target | setting target");
                    theTarget = value;
                    transform.LookAt(theTarget);
                    NewTargetAcquired?.Invoke(theTarget);
                }
            }
        }

        private BehaviorTree _tree;
        private float _lastAttackTime;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<EnemyConfiguration>();
            _tree = config.BuildBehaviorTree(gameObject);
            _lastAttackTime = Time.time - config.AttackDelay;
        }

        public TaskStatus MoveToTarget()
        {
            DebugLog($"<BasicEnemy> | MoveToTarget() | (Target {target}, reached target? {ReachedTarget()})");
            if (target == null) return TaskStatus.Failure;

            if (ReachedTarget()) return TaskStatus.Success;

            transform.position = Vector3.MoveTowards(
                transform.position,
                target.position,
                config.MoveSpeed * Time.deltaTime
            );
            MovedTowardsPosition?.Invoke(target.position);
            return TaskStatus.Continue;
        }

        private bool ReachedTarget()
        {
            DebugLog($"ReachedTarget | (distance {Vector3.Distance(target.position, transform.position)})");
            return Vector3.Distance(target.position, transform.position) < 0.3f;
        }

        public bool HasTarget()
        {
            DebugLog($"HasTarget | (Target != null {target != null})");
            return target != null;
        }

        private void Update()
        {
            _tree.Tick();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_damageable != null) return;

            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                _damageable = damageable;
                DiscoveredDamageable?.Invoke(_damageable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_damageable == null) return;

            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (_damageable == damageable)
                {
                    _damageable = null;
                    ForgotDamageable?.Invoke(damageable);
                }
            }
        }

        public bool HasPath()
        {
            DebugLog($"HasPath | knows about path? {path != null}");
            return path != null;
        }

        public TaskStatus ForgetPath()
        {
            if (path == null) return TaskStatus.Failure;
            ForgottenPath?.Invoke(path);
            path = null;
            return TaskStatus.Success;
        }

        public TaskStatus FollowPath()
        {
            DebugLog($"FollowPath | (# of path points {path.pathPoints.Count}, currentPointIndex {_currentPathPointIndex})");
            if (path == null || path.pathPoints.Count == 0)
            {
                DebugLog($"FollowPath | path is null or path points == 0; failure");
                return TaskStatus.Failure;
            }

            if (_currentPathPointIndex == path.pathPoints.Count)
            {
                DebugLog($"FollowPath | successfully followed path");
                // reset internal state
                _currentPathPointIndex = 0;
                return TaskStatus.Success;
            }

            var currentPoint = path.pathPoints[_currentPathPointIndex];
            DebugLog($"FollowPath | current point location {currentPoint.position}");
            if (target == null || !target.Equals(currentPoint))
            {
                var oldPosition = target == null ? Vector3.zero : target.position;
                DebugLog($"<BasicEnemy> | FollowPath | setting new target (old: {oldPosition}, new: {currentPoint.position}");
                target = currentPoint;
            }

            var status = MoveToTarget();
            if (status == TaskStatus.Success)
            {
                ReachedPoint?.Invoke(target.gameObject);
                _currentPathPointIndex++;
            }

            return status == TaskStatus.Success ? TaskStatus.Continue : status;
        }

        private void DebugLog(string logMessage)
        {
            if (debugEnabled) Debug.Log($"<BasicEnemy> | {logMessage}");
        }

        public event Action<Path> ForgottenPath;

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
            DebugLog($"AttackDamageable | Attacking damageable");
            _damageable.Damage(config.AttackDamage);
            DamageableAttacked?.Invoke(_damageable);
            _lastAttackTime = Time.time;
            return TaskStatus.Success;
        }
    }
}