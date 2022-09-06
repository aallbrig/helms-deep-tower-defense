using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoBehaviours;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyAndAPathToFollow
    {
        private void Setup(in List<GameObject> destroyList, out GameObject enemy, out GameObject path)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Basic Enemy"));
            enemy.name = "System Under Test (sut)";
            destroyList.Add(enemy);

            path = new GameObject();
            path.name = "Test path";
            var pathComponent = path.AddComponent<Path>();
            pathComponent.pathPoints.Add(
                new GameObject{ name = "Test path point [0]", transform = { position = new Vector3(0, 0, 10) }}.transform
            );
            destroyList.AddRange(pathComponent.pathPoints.ToList().Select(tr => tr.gameObject));
            destroyList.Add(path);

            testCamera.transform.LookAt(enemy.transform);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanFollow_APath()
        {
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var enemy, out var path);

            var recordedPosition = Vector3.zero;
            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.path = path.GetComponent<Path>();
            basicEnemyScript.MovedTowardsPosition += targetPosition => recordedPosition = targetPosition;
            yield return null;

            Assert.AreEqual(path.GetComponent<Path>().pathPoints[0].position, recordedPosition);

            Teardown(destroyList);
        }
    }
}