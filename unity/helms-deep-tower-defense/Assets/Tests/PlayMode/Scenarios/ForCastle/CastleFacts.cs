﻿using System.Collections;
using System.Collections.Generic;
using Model.Combat;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForCastle
{
    public class CastleFacts
    {
        private void Setup(in List<GameObject> destroyList, out GameObject castle)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            castle = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Castle"));
            castle.name = "System Under Test (sut)";
            destroyList.Add(castle);

            testCamera.transform.LookAt(castle.transform);
        }

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
            Setup(destroyList, out var castle);

            yield return null;

            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            Assert.NotNull(castleComponent, "component exists");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Castle_UsesA_Trigger()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var castle);

            yield return null;

            var component = castle.GetComponent<BoxCollider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Attacked()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var castle);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var damaged = false;
            castleComponent.Damaged += _ => damaged = true;
            yield return null;

            castleComponent.Damage(1);

            Assert.IsTrue(damaged);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Killed()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var castle);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var killed = false;
            castleComponent.Killed += () => killed = true;
            yield return null;

            castleComponent.Damage(castleComponent.maxHealth);

            Assert.IsTrue(killed);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Castle_CanAssign_AttackPoint()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var castle);
            var component = castle.GetComponent<IAssignAttackPoints>();
            yield return null;

            var attackPointTransform = component.AssignAttackPoint();
            
            Assert.NotNull(attackPointTransform);
        }
    }
}