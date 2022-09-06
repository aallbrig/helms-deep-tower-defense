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

        public EnemyConfiguration config;
        public Transform target;
        private BehaviorTree _tree;

        private void Awake()
        {
            _tree = new BehaviorTreeBuilder(gameObject)
                .Selector()
                    .Sequence()
                        .Condition(HasTarget)
                        .Do(MoveToTarget)
                    .End()
                .End()
                .Build();
        }

        private TaskStatus MoveToTarget()
        {
            if (target == null) return TaskStatus.Failure;
            
            if (Vector3.Distance(target.position, transform.position) < 0.3f)
                return TaskStatus.Success;

            transform.position = Vector3.MoveTowards(transform.position, target.position, config.moveSpeed * Time.deltaTime);
            MovedTowardsPosition?.Invoke(target.position);
            return TaskStatus.Continue;
        }

        private bool HasTarget() => target != null;

        private void Update()
        {
            _tree.Tick();
        }
    }
}