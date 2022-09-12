﻿using System.Collections;
using System.Collections.Generic;
using Model.Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForTowers
{
    public class TowerFacts
    {
        private void Setup(in List<GameObject> destroyList, out GameObject tower)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            tower = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Tower"));
            tower.name = "System Under Test (sut)";
            destroyList.Add(tower);

            testCamera.transform.LookAt(tower.transform);
        }

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
            Setup(destroyList, out var tower);

            yield return null;

            var towerComponent = tower.GetComponent<Tower>();
            Assert.NotNull(towerComponent, "component exists");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_UsesA_Trigger()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var tower);

            yield return null;

            var component = tower.GetComponent<BoxCollider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_CanBe_Attacked()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var tower);
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
            Setup(destroyList, out var tower);
            var towerComponent = tower.GetComponent<Tower>();
            var killed = false;
            towerComponent.Killed += () => killed = true;
            yield return null;

            towerComponent.Damage(towerComponent.maxHealth);

            Assert.IsTrue(killed);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Tower_CanAssign_AttackPoint()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var tower);
            var component = tower.GetComponent<IAssignAttackPoints>();
            yield return null;

            var attackPointTransform = component.AssignAttackPoint();
            
            Assert.NotNull(attackPointTransform);
        }
    }
}