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
    public class TowerFacts: ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Tower");

        [UnityTest]
        public IEnumerator Tower_UsesA_TowerComponent()
        {
            var tower = _prefabSpawner.Spawn();
            CleanupAtEnd(tower);
            TestCameraLookAt(tower.transform);
            yield return null;

            var towerComponent = tower.GetComponent<Tower>();
            Assert.NotNull(towerComponent, "component exists");
        }

        [UnityTest]
        public IEnumerator Tower_UsesA_Trigger()
        {
            var tower = _prefabSpawner.Spawn();
            CleanupAtEnd(tower);
            TestCameraLookAt(tower.transform);
            yield return null;

            var component = tower.GetComponent<Collider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");
        }

        [UnityTest]
        public IEnumerator Tower_CanBe_Attacked()
        {
            var tower = _prefabSpawner.Spawn();
            CleanupAtEnd(tower);
            TestCameraLookAt(tower.transform);
            var towerComponent = tower.GetComponent<Tower>();
            var damaged = false;
            towerComponent.Damaged += _ => damaged = true;
            yield return null;

            towerComponent.Damage(1);

            Assert.IsTrue(damaged);
        }

        [UnityTest]
        public IEnumerator Tower_CanBe_Killed()
        {
            var tower = _prefabSpawner.Spawn();
            CleanupAtEnd(tower);
            TestCameraLookAt(tower.transform);
            var towerComponent = tower.GetComponent<Tower>();
            var killed = false;
            towerComponent.Killed += () => killed = true;
            yield return null;

            var configFacade = tower.GetComponent<ITowerConfig>();
            towerComponent.Damage(configFacade.MaxHealth);

            Assert.IsTrue(killed);
        }
    }
}