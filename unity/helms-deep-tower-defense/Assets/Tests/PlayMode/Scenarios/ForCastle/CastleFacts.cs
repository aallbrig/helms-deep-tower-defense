using System.Collections;
using System.Collections.Generic;
using Model.Combat;
using Model.Factories;
using Model.Factories.Camera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForCastle
{
    public class CastleFacts
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Castle");

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator Castle_UsesA_CastleComponent()
        {
            var destroyList = new List<GameObject>();
            var castle = _prefabSpawner.Spawn();
            destroyList.Add(castle);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(castle.transform);
            yield return null;

            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            Assert.NotNull(castleComponent, "component exists");

            Teardown(destroyList);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Castle_UsesA_Trigger()
        {
            var destroyList = new List<GameObject>();
            var castle = _prefabSpawner.Spawn();
            destroyList.Add(castle);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(castle.transform);
            yield return null;

            var component = castle.GetComponent<BoxCollider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");

            Teardown(destroyList);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Attacked()
        {
            var destroyList = new List<GameObject>();
            var castle = _prefabSpawner.Spawn();
            destroyList.Add(castle);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(castle.transform);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var damaged = false;
            castleComponent.Damaged += _ => damaged = true;
            yield return null;

            castleComponent.Damage(1);

            Assert.IsTrue(damaged);

            Teardown(destroyList);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Killed()
        {
            var destroyList = new List<GameObject>();
            var castle = _prefabSpawner.Spawn();
            destroyList.Add(castle);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(castle.transform);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var killed = false;
            castleComponent.Killed += () => killed = true;
            yield return null;

            castleComponent.Damage(castleComponent.maxHealth);

            Assert.IsTrue(killed);

            Teardown(destroyList);
            yield return null;
        }

        [UnityTest]
        public IEnumerator Castle_CanAssign_AttackPoint()
        {
            var destroyList = new List<GameObject>();
            var castle = _prefabSpawner.Spawn();
            destroyList.Add(castle);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(castle.transform);
            var component = castle.GetComponent<IAssignAttackPoints>();
            yield return null;

            var attackPointTransform = component.AssignAttackPoint();
            
            Assert.NotNull(attackPointTransform);
            Teardown(destroyList);
            yield return null;
        }
    }
}