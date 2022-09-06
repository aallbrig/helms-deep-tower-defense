using System.Collections;
using System.Collections.Generic;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyAndAPathToFollow
    {
        private void Setup(in List<GameObject> destroyList, out GameObject sutInstance)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            sutInstance = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Basic Enemy"));
            sutInstance.name = "System Under Test (sut)";
            destroyList.Add(sutInstance);

            testCamera.transform.LookAt(sutInstance.transform);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanRunTowards_ATarget()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy);

            var expectedPosition = new Vector3(0, 0, 30);
            var recordedPosition = Vector3.zero;
            var path = new GameObject { transform = { position = expectedPosition}};
            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.target = path.transform;
            basicEnemyScript.MovedTowardsPosition += targetPosition => recordedPosition = targetPosition;
            yield return null;

            Assert.AreEqual(expectedPosition, recordedPosition);

            Teardown(destroyList);
        }
    }
}