using System;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.AI
{
    public class BasicEnemy : MonoBehaviour
    {
        public event Action<Vector3> MovedTowardsPosition;
        public event Action<GameObject> ReachedPoint;

        public EnemyConfiguration config;
        public Path path;
        private int _currentPathPointIndex;
        public Transform target;
        private BehaviorTree _tree;

        private void Awake()
        {
            config ??= ScriptableObject.CreateInstance<EnemyConfiguration>();
            _tree = config.BuildBehaviorTree(gameObject);
        }

        public TaskStatus MoveToTarget()
        {
            if (target == null) return TaskStatus.Failure;
            
            if (ReachedTarget())
                return TaskStatus.Success;

            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, config.MoveSpeed * Time.deltaTime);
            MovedTowardsPosition?.Invoke(target.position);
            return TaskStatus.Continue;
        }

        private bool ReachedTarget()
        {
            return Vector3.Distance(target.position, transform.position) < 0.3f;
        }

        public bool HasTarget() => target != null;

        private void Update()
        {
            _tree.Tick();
        }
        public bool HasPath() => path != null;

        public TaskStatus FollowPath()
        {
            if (_currentPathPointIndex >= path.pathPoints.Count)
            {
                // reset internal state
                _currentPathPointIndex = 0;
                return TaskStatus.Success;
            }
            var currentPoint = path.pathPoints[_currentPathPointIndex];
            if (!target.Equals(currentPoint))
            {
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
    }
}