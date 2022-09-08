using System.Collections;
using System.Collections.Generic;
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
            Assert.NotNull(castleComponent.hitBox, "has a hit box");

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator Castle_CanBe_Attacked()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var castle);
            var castleComponent = castle.GetComponent<MonoBehaviours.Castle>();
            var damaged = false;
            castleComponent.Damaged += () => damaged = true;
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
    }
}