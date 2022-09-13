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
    public class BasicEnemyAndATarget
    {
        private class TestSetupConfig
        {
            public Vector3 CameraPosition = new Vector3(0, -10, -10);
            public Vector3 BasicEnemyPosition = new Vector3(0, 0, 0);
            public Vector3 TargetPosition = new Vector3(0, 0, 10);
        }
        private void Setup(TestSetupConfig config, in List<GameObject> destroyList, out GameObject enemy, out GameObject target)
        {
            var testCamera = new GameObject { transform = { position = config.CameraPosition } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Basic Enemy"));
            enemy.transform.position = config.BasicEnemyPosition;
            enemy.name = "Test basic enemy";
            destroyList.Add(enemy);

            target = new GameObject { transform = { position = config.TargetPosition } };
            target.name = "Test target";
            destroyList.Add(target);

            testCamera.transform.LookAt(enemy.transform);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanMoveTowards_Target()
        {
            var destroyList = new List<GameObject>();
            Setup(new TestSetupConfig(), destroyList, out var enemy, out var target);

            var recordedPosition = Vector3.zero;
            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.theTarget = target.transform;
            basicEnemyScript.MovedTowardsPosition += targetPosition => recordedPosition = targetPosition;
            yield return null;

            Assert.AreEqual(target.transform.position, recordedPosition);

            Teardown(destroyList);
            yield return null;
        }

        [UnityTest]
        public IEnumerator BasicEnemy_LooksAt_Target()
        {
            var destroyList = new List<GameObject>();
            Setup(new TestSetupConfig { TargetPosition = new Vector3(10, 0, 10)}, destroyList, out var enemy, out var target);

            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.Target = target.transform;
            yield return null;

            Assert.IsTrue(enemy.transform.forward.normalized == (target.transform.position - enemy.transform.position).normalized);

            Teardown(destroyList);
            yield return null;
        }
    }
}