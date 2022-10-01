using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Factories;
using Model.Factories.Camera;
using MonoBehaviours.AI;
using MonoBehaviours.Combat;
using MonoBehaviours.Commerce;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyFacts: ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Enemies/Basic Enemy");

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Animator()
        {
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            yield return null;

            var animatorComponent = enemy.GetComponent<Animator>();
            var animator = animatorComponent.runtimeAnimatorController;
            var animationClips = animator.animationClips;
            var hasMovementClip = animationClips.ToList().Aggregate(false, (acc, clip) => acc == true ? acc : clip.name == "basic enemy movement");

            Assert.NotNull(animatorComponent, "Animator component exists");
            Assert.NotNull(animator, "Animator has a controller asset");
            Assert.IsTrue(hasMovementClip, "Basic movement animation included in animator");
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_RigidBody()
        {
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            yield return null;

            var rigidBodyComponent = enemy.GetComponent<Rigidbody>();

            Assert.NotNull(rigidBodyComponent, "rigid body component exists");
            Assert.IsFalse(rigidBodyComponent.useGravity, "does not use gravity");
            Assert.IsTrue(rigidBodyComponent.isKinematic, "basic enemies can move through each other (but still be able to trigger a trigger");
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Collider()
        {
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            yield return null;

            var collider = enemy.GetComponent<BoxCollider>();

            Assert.NotNull(collider, "collider component exists");
        }
        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Reward()
        {
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            yield return null;
            Assert.IsTrue(enemy.TryGetComponent<IRewardMoney>(out _), $"enemy requires IRewardMoney");
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Damageable_OnTriggerEnter()
        {
            var damageableDiscovered = false;
            var damaged = false;
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            enemy.GetComponent<BasicEnemy>().DiscoveredDamageable += _ => damageableDiscovered = true;

            var mockDamageable = new GameObject
            {
                name = "Mock damageable",
                transform = { position = enemy.transform.position }
            };
            var damageableComponent = mockDamageable.AddComponent<MonoBehaviours.Castle>();
            damageableComponent.Damaged += _ => damaged = true;
            var collider = mockDamageable.AddComponent<BoxCollider>();
            collider.size = new Vector3(3, 3, 3);
            collider.isTrigger = true;

            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(damageableDiscovered, "damageable discovered");
            Assert.IsTrue(damaged, "damageable attacked");
        }

        [UnityTest]
        public IEnumerator BasicEnemy_ForgetsA_Damageable_OnTriggerExit()
        {
            var damageableForgotten = false;
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            enemy.GetComponent<BasicEnemy>().ForgotDamageable += _ => damageableForgotten = true;

            var mockDamageable = new GameObject
            {
                name = "Mock damageable",
                transform = { position = enemy.transform.position }
            };
            var damageableComponent = mockDamageable.AddComponent<MonoBehaviours.Castle>();
            var collider = mockDamageable.AddComponent<BoxCollider>();
            collider.size = new Vector3(3, 3, 3);
            collider.isTrigger = true;
            yield return new WaitForSeconds(0.1f);

            mockDamageable.transform.position = enemy.transform.position + new Vector3(0, 0, 100);
            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(damageableForgotten, "damageable forgotten when not touching");
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanAcquireAttackPoint_OnTriggerEnter()
        {
            var attackPointAcquired = false;
            var enemy = _prefabSpawner.Spawn();
            TestCameraLookAt(enemy.transform);
            enemy.GetComponent<BasicEnemy>().AttackPointAcquired += _ => attackPointAcquired = true;

            var mockAssignAttackPoint = new GameObject
            {
                name = "Mock damageable",
                transform = { position = enemy.transform.position + new Vector3(0, 0, 100) }
            };
            var component = mockAssignAttackPoint.AddComponent<AttackPoints>();
            var collider = mockAssignAttackPoint.AddComponent<BoxCollider>();
            collider.size = new Vector3(3, 3, 3);
            collider.isTrigger = true;
            yield return null;

            mockAssignAttackPoint.transform.position = enemy.transform.position;
            yield return new WaitForSeconds(0.1f);

            Assert.IsTrue(attackPointAcquired, "acquired attack point from trigger");
        }
    }
}
