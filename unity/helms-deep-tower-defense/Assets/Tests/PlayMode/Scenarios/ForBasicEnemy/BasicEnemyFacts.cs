using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyFacts
    {
        private void Setup(in List<GameObject> destroyList, out GameObject enemy)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Basic Enemy"));
            enemy.name = "System Under Test (sut)";
            enemy.GetComponent<BasicEnemy>().debugEnabled = true;
            destroyList.Add(enemy);

            testCamera.transform.LookAt(enemy.transform);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Animator()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);

            yield return null;

            var animatorComponent = enemy.GetComponent<Animator>();
            var animator = animatorComponent.runtimeAnimatorController;
            var animationClips = animator.animationClips;
            var hasMovementClip = animationClips.ToList().Aggregate(false, (acc, clip) => acc == true ? acc : clip.name == "basic enemy movement");

            Assert.NotNull(animatorComponent, "Animator component exists");
            Assert.NotNull(animator, "Animator has a controller asset");
            Assert.IsTrue(hasMovementClip, "Basic movement animation included in animator");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_RigidBody()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);

            yield return null;

            var rigidBodyComponent = enemy.GetComponent<Rigidbody>();

            Assert.NotNull(rigidBodyComponent, "rigid body component exists");
            Assert.IsFalse(rigidBodyComponent.useGravity, "does not use gravity");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Collider()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);

            yield return null;

            var collider = enemy.GetComponent<BoxCollider>();

            Assert.NotNull(collider, "collider component exists");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Damageable_OnTriggerEnter()
        {
            var damageableDiscovered = false;
            var damaged = false;
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);
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
            destroyList.Add(mockDamageable);

            yield return null;
            yield return null;

            Assert.IsTrue(damageableDiscovered, "damageable discovered");
            Assert.IsTrue(damaged, "damageable attacked");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_ForgetsA_Damageable_OnTriggerExit()
        {
            var damageableForgotten = false;
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);
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
            destroyList.Add(mockDamageable);

            yield return null;
            yield return null;
            damageableComponent.transform.position = enemy.transform.position + new Vector3(0, 100, 0);
            yield return null;
            yield return null;

            Assert.IsTrue(damageableForgotten, "damageable forgotten when not touching");

            Teardown(destroyList);
        }
    }
}