using System.Collections;
using System.Collections.Generic;
using Model.Combat;
using Model.Factories;
using Model.Factories.Camera;
using MonoBehaviours;
using NUnit.Framework;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForTowers
{
    public class TowerFacts
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Tower");

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator Tower_UsesA_TowerComponent()
        {
            var destroyList = new List<GameObject>();
            var tower = _prefabSpawner.Spawn();
            destroyList.Add(tower);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(tower.transform);
            yield return null;

            var towerComponent = tower.GetComponent<Tower>();
            Assert.NotNull(towerComponent, "component exists");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_UsesA_Trigger()
        {
            var destroyList = new List<GameObject>();
            var tower = _prefabSpawner.Spawn();
            destroyList.Add(tower);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(tower.transform);

            yield return null;

            var component = tower.GetComponent<Collider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_CanBe_Attacked()
        {
            var destroyList = new List<GameObject>();
            var tower = _prefabSpawner.Spawn();
            destroyList.Add(tower);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(tower.transform);
            var towerComponent = tower.GetComponent<Tower>();
            var damaged = false;
            towerComponent.Damaged += _ => damaged = true;
            yield return null;

            towerComponent.Damage(1);

            Assert.IsTrue(damaged);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_CanBe_Killed()
        {
            var destroyList = new List<GameObject>();
            var tower = _prefabSpawner.Spawn();
            destroyList.Add(tower);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(tower.transform);
            var towerComponent = tower.GetComponent<Tower>();
            var killed = false;
            towerComponent.Killed += () => killed = true;
            yield return null;

            var configFacade = tower.GetComponent<ITowerConfig>();
            towerComponent.Damage(configFacade.MaxHealth);

            Assert.IsTrue(killed);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_CanAssign_AttackPoint()
        {
            var destroyList = new List<GameObject>();
            var tower = _prefabSpawner.Spawn();
            destroyList.Add(tower);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(tower.transform);
            var component = tower.GetComponent<IAssignAttackPoints>();
            yield return null;

            var attackPointTransform = component.AssignAttackPoint();
            
            Assert.NotNull(attackPointTransform);
            Teardown(destroyList);
        }
    }
}