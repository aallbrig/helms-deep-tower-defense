using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.AI
{
    public class BasicEnemy : MonoBehaviour
    {
        public event Action<Transform> NewTargetAcquired;
        public event Action<Vector3> MovedTowardsPosition;
        public event Action<GameObject> ReachedPoint;

        public EnemyConfiguration config;
        public Path path;
        private int _currentPathPointIndex = 0;
        public Transform target;
        public bool debugEnabled = false;

        private Transform Target
        {
            get => target;
            set
            {
                if (target == null || !target.Equals(value))
                {
                    DebugLog("Target | setting target");
                    target = value;
                    transform.LookAt(target);
                    NewTargetAcquired?.Invoke(target);
                }
            }
        }

        private BehaviorTree _tree;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<EnemyConfiguration>();
            _tree = config.BuildBehaviorTree(gameObject);
        }

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

        private void Update()
        {
            _tree.Tick();
        }
        public bool HasPath()
        {
            DebugLog($"HasPath | (path != null {path != null})");
            return path != null;
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
    }
}