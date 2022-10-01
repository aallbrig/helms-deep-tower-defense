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
    public class CastleFacts:ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Castle");

        [UnityTest]
        public IEnumerator Castle_UsesA_CastleComponent()
        {
            var castle = _prefabSpawner.Spawn();
            TestCameraLookAt(castle.transform);
            yield return null;

            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            Assert.NotNull(castleComponent, "component exists");
        }

        [UnityTest]
        public IEnumerator Castle_UsesA_Trigger()
        {
            var castle = _prefabSpawner.Spawn();
            TestCameraLookAt(castle.transform);
            yield return null;

            var component = castle.GetComponent<BoxCollider>();
            Assert.NotNull(component, "collider exists");
            Assert.NotNull(component.isTrigger, "collider is a trigger");
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Attacked()
        {
            var castle = _prefabSpawner.Spawn();
            TestCameraLookAt(castle.transform);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var damaged = false;
            castleComponent.Damaged += _ => damaged = true;
            yield return null;

            castleComponent.Damage(1);

            Assert.IsTrue(damaged);
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Killed()
        {
            var castle = _prefabSpawner.Spawn();
            TestCameraLookAt(castle.transform);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var killed = false;
            castleComponent.Killed += () => killed = true;
            yield return null;

            castleComponent.Damage(castleComponent.maxHealth);

            Assert.IsTrue(killed);
        }

        [UnityTest]
        public IEnumerator Castle_CanAssign_AttackPoint()
        {
            var castle = _prefabSpawner.Spawn();
            TestCameraLookAt(castle.transform);
            var component = castle.GetComponentInChildren<IAssignAttackPoints>();
            yield return null;

            var attackPointTransform = component.AssignAttackPoint();

            Assert.NotNull(attackPointTransform);
        }
    }
}
