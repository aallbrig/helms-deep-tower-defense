using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.Factories;
using Model.Factories.Camera;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyFacts
    {
        private readonly TestCamera _testCameraSpawner = new TestCamera(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Basic Enemy");

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
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
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
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
            yield return null;

            var rigidBodyComponent = enemy.GetComponent<Rigidbody>();

            Assert.NotNull(rigidBodyComponent, "rigid body component exists");
            Assert.IsFalse(rigidBodyComponent.useGravity, "does not use gravity");
            Assert.IsTrue(rigidBodyComponent.isKinematic, "basic enemies can move through each other (but still be able to trigger a trigger");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_Collider()
        {
            var destroyList = new List<GameObject>();
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
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
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
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
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
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
        [UnityTest]
        public IEnumerator BasicEnemy_UsesA_AssignAttackPoint_OnTriggerEnter()
        {
            var attackPointAcquired = false;
            var destroyList = new List<GameObject>();
            var enemy = _prefabSpawner.Spawn();
            destroyList.Add(enemy);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(enemy.transform);
            enemy.GetComponent<BasicEnemy>().AttackPointAcquired += _ => attackPointAcquired = true;

            var mockAssignAttackPoint = new GameObject
            {
                name = "Mock damageable",
                transform = { position = enemy.transform.position }
            };
            var component = mockAssignAttackPoint.AddComponent<MonoBehaviours.Castle>();
            var collider = mockAssignAttackPoint.AddComponent<BoxCollider>();
            collider.size = new Vector3(3, 3, 3);
            collider.isTrigger = true;
            destroyList.Add(mockAssignAttackPoint);

            yield return null;
            yield return null;

            Assert.IsTrue(attackPointAcquired, "acquired attack point from trigger");

            Teardown(destroyList);
        }
    }
}