using System;
using CleverCrow.Fluid.BTs.Trees;
using Model.AI;
using MonoBehaviours;
using UnityEngine;

namespace ScriptableObjects
{
    public interface ITowerConfig
    {
        public float MaxHealth { get; }

        public float Range { get; }
    }

    [CreateAssetMenu(fileName = "new tower config", menuName = "Game/tower config", order = 0)]
    public class TowerConfiguration : ScriptableObject, ITowerConfig, IBehaviorTreeBuilder<GameObject>
    {
        public float maxHealth = 3.0f;
        public float range = 5.0f;
        public float damage = 0.5f;
        public float senseDelay = 0.4f;
        public float attackDelay = 1.1f;
        public GameObject projectilePrefab;

        public BehaviorTree BuildBehaviorTree(GameObject context)
        {
            var tower = context.GetComponent<Tower>();
            if (tower == null) throw new NullReferenceException();
            // If enemy has a path to follow, follow the path.
            // otherwise, if the enemy has a target to go to, go to target
            var attackTargetSequence = new BehaviorTreeBuilder(context)
                .Sequence()
                .Condition(tower.CanSenseTargets)
                .Do(tower.SenseForTargets)
                .Condition(tower.HasTargets)
                .Do(tower.FindClosestTarget)
                .Do(tower.LookAtClosestTarget)
                .Condition(tower.CanAttackTarget)
                .Do(tower.AttackTarget)
                .End();
            return new BehaviorTreeBuilder(context)
                .Selector()
                .Splice(attackTargetSequence.Build())
                .End()
                .Build();
        }

        public float MaxHealth => maxHealth;

        public float Range => range;
    }
}