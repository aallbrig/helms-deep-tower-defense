using CleverCrow.Fluid.BTs.Trees;
using Model.AI;
using MonoBehaviours.AI;
using UnityEngine;

namespace ScriptableObjects
{

    public interface IEnemyConfig
    {
        public float MoveSpeed { get; }
        public float AttackDelay { get; }
        public float AttackDamage { get; }
    }

    [CreateAssetMenu(fileName = "enemy configuration", menuName = "Game/new enemy configuration", order = 0)]
    public class EnemyConfiguration : ScriptableObject, IEnemyConfig, IBehaviorTreeBuilder<GameObject>
    {
        public float moveSpeed = 3.5f;
        public float attackDelay = 2.0f;
        public float attackDamage = 1.0f;

        public BehaviorTree BuildBehaviorTree(GameObject context)
        {
            var basicEnemy = context.GetComponent<BasicEnemy>();
            // If enemy has a path to follow, follow the path.
            // otherwise, if the enemy has a target to go to, go to target
            return new BehaviorTreeBuilder(context)
                .Selector()
                    .Sequence()
                        .Condition(basicEnemy.HasDamageable)
                        .Condition(basicEnemy.CanAttackDamageable)
                        .Do(basicEnemy.AttackDamageable)
                    .End()
                    .Sequence()
                        .Condition(basicEnemy.HasPath)
                        .Do(basicEnemy.FollowPath)
                        .Do(basicEnemy.ForgetPath)
                    .End()
                    .Sequence()
                        .Condition(basicEnemy.HasTarget)
                        .Do(basicEnemy.MoveToTarget)
                    .End()
                .End()
                .Build();
        }

        public float MoveSpeed => moveSpeed;
        public float AttackDelay => attackDelay;
        public float AttackDamage => attackDamage;
    }
}