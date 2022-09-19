using System.Collections;
using Model.Factories;
using MonoBehaviours.Combat;
using NUnit.Framework;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Tests.PlayMode.Scenarios.ForProjectiles
{
    public class ProjectileFacts : ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Basic Projectile");
        [UnityTest]
        public IEnumerator Projectile_UsesA_RigidBodyComponent()
        {
            var projectile = _prefabSpawner.Spawn();
            CleanupAtEnd(projectile);
            TestCameraLookAt(projectile.transform);
            yield return null;
            var rigidBody = projectile.GetComponent<Rigidbody>();
            Assert.NotNull(rigidBody, "projectile does not have the rigid body component");
            Assert.IsFalse(rigidBody.useGravity, "rigid body should not use gravity");
        }
        [UnityTest]
        public IEnumerator Projectile_UsesA_ColliderComponent()
        {
            var projectile = _prefabSpawner.Spawn();
            CleanupAtEnd(projectile);
            TestCameraLookAt(projectile.transform);
            yield return null;
            var collider = projectile.GetComponent<SphereCollider>();
            Assert.NotNull(collider, "projectile does not have a collider component");
            Assert.IsTrue(collider.isTrigger, "projectile collider needs to be a trigger");
        }
        [UnityTest]
        public IEnumerator Projectile_UsesA_ProjectileComponent()
        {
            var projectile = _prefabSpawner.Spawn();
            CleanupAtEnd(projectile);
            TestCameraLookAt(projectile.transform);
            yield return null;
            var projectileComponent = projectile.GetComponent<Projectile>();
            Assert.NotNull(projectileComponent, "projectile does not have the projectile component");
        }
        [UnityTest]
        public IEnumerator Projectile_MovesForward()
        {
            var projectile = _prefabSpawner.Spawn();
            CleanupAtEnd(projectile);
            TestCameraLookAt(projectile.transform);
            var previousPosition = projectile.transform.localPosition;
            yield return null;
            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(previousPosition.z < projectile.transform.localPosition.z, "project needs to move forward");
        }
        [UnityTest]
        public IEnumerator Projectile_Use_ProjectileConfiguration()
        {
            var teamConfiguration = ScriptableObject.CreateInstance<TeamConfiguration>();
            var projectileConfiguration = ScriptableObject.CreateInstance<ProjectileConfiguration>();
            projectileConfiguration.projectileSpeed = 69f;
            var projectile = _prefabSpawner.Spawn();
            CleanupAtEnd(projectile);
            TestCameraLookAt(projectile.transform);
            var projectileComponent = projectile.GetComponent<Projectile>();
            yield return null;

            projectileComponent.config = projectileConfiguration;

            Assert.NotNull(projectileComponent.Config, "IProjectileConfiguration interface needs to be set");
            Assert.IsTrue(Utils.AreFloatsEqual(projectileComponent.Config.speed, projectileConfiguration.speed, 10e-6f));
        }
    }
}