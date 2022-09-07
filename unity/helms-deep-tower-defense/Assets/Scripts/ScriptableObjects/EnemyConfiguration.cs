using CleverCrow.Fluid.BTs.Trees;
using MonoBehaviours.AI;
using UnityEngine;

namespace ScriptableObjects
{
    public interface IBehaviorTreeBuilder<in T>
    {
        public BehaviorTree BuildBehaviorTree(T context);
    }

    public interface IEnemyConfig
    {
        public float MoveSpeed { get; }
    }

    [CreateAssetMenu(fileName = "enemy configuration", menuName = "Game/new enemy configuration", order = 0)]
    public class EnemyConfiguration : ScriptableObject, IEnemyConfig, IBehaviorTreeBuilder<GameObject>
    {
        public float moveSpeed = 3.5f;

        public BehaviorTree BuildBehaviorTree(GameObject context)
        {
            var basicEnemy = context.GetComponent<BasicEnemy>();
            // If enemy has a path to follow, follow the path.
            // otherwise, if the enemy has a target to go to, go to target
            return new BehaviorTreeBuilder(context)
                .Selector()
                    .Sequence()
                        .Condition(basicEnemy.HasPath)
                        .Do(basicEnemy.FollowPath)
                    .End()
                    .Sequence()
                        .Condition(basicEnemy.HasTarget)
                        .Do(basicEnemy.MoveToTarget)
                    .End()
                .End()
                .Build();
        }

        public float MoveSpeed => moveSpeed;
    }
}