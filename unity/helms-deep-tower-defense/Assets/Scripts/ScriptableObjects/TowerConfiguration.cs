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
        public float timeBetweenAttacks = 1.2f;

        public float MaxHealth => maxHealth;
        public float Range => range;

        public BehaviorTree BuildBehaviorTree(GameObject context)
        {
            var tower = context.GetComponent<Tower>();
            if (tower == null) throw new NullReferenceException();
            // If enemy has a path to follow, follow the path.
            // otherwise, if the enemy has a target to go to, go to target
            var attackTargetSequence = new BehaviorTreeBuilder(context)
                .Sequence()
                    .Condition(tower.HasTarget)
                    .Do(tower.AttackTarget)
                    .WaitTime(timeBetweenAttacks)
                .End();
            return new BehaviorTreeBuilder(context)
                .Selector()
                    .Splice(attackTargetSequence.Build())
                .End()
                .Build();
        }
    }
}