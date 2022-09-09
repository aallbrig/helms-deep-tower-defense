using System.Collections;
using System.Collections.Generic;
using MonoBehaviours;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyAndAPathToFollow
    {
        private void SetupCamera(in List<GameObject> destroyList, out GameObject camera)
        {
            camera = new GameObject
            {
                name = "test scenario camera",
                tag = "MainCamera",
                transform = { position = new Vector3(0, 10, -10) }
            };
            camera.AddComponent<Camera>();
            destroyList.Add(camera);
        }
        private void SetupEnemy(in List<GameObject> destroyList, out GameObject enemy)
        {
            enemy = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Basic Enemy"));
            enemy.name = $"{enemy.name} (test)";
            enemy.GetComponent<BasicEnemy>().debugEnabled = true;
            destroyList.Add(enemy);
        }
        private void SetupPath(in List<GameObject> destroyList, out GameObject path)
        {
            path = new GameObject();
            path.name = "Test path";
            destroyList.Add(path);
        }

        private void SetupPathPoint(in List<GameObject> destroyList, in GameObject path, Vector3 pathPointPosition)
        {
            var pathComponent = path.AddComponent<Path>();
            var newPathPoint = new GameObject
            {
                name = $"Test path point [{pathComponent.pathPoints.Count}]",
                transform = { position = pathPointPosition }
            };
            destroyList.Add(newPathPoint);
            pathComponent.pathPoints.Add(newPathPoint.transform);
        }

        private void Setup(in List<GameObject> destroyList, out GameObject enemy, out GameObject path)
        {
            SetupCamera(destroyList, out var camera);
            SetupEnemy(destroyList, out enemy);
            camera.transform.LookAt(enemy.transform);
            SetupPath(destroyList, out path);
            SetupPathPoint(destroyList, path, new Vector3(0, 0, 10));
        }

        private void Teardown(List<GameObject> gameObjectsToDestroy)
        {
            for (var i = gameObjectsToDestroy.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjectsToDestroy[i]);
            gameObjectsToDestroy.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanFollow_APath()
        {
            var destroyList = new List<GameObject>();
            SetupCamera(destroyList, out var camera);
            SetupEnemy(destroyList, out var enemy);
            camera.transform.LookAt(enemy.transform);
            SetupPath(destroyList, out var path);
            SetupPathPoint(destroyList, path, new Vector3(0, 0, 10f));

            var recordedPosition = Vector3.zero;
            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.path = path.GetComponent<Path>();
            basicEnemyScript.MovedTowardsPosition += targetPosition => recordedPosition = targetPosition;

            yield return null;

            Assert.AreEqual(path.GetComponent<Path>().pathPoints[0].position, recordedPosition);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator BasicEnemy_ForgetsAboutPath_OnPathCompletion()
        {
            var destroyList = new List<GameObject>();
            SetupCamera(destroyList, out var camera);
            SetupEnemy(destroyList, out var enemy);
            camera.transform.LookAt(enemy.transform);
            SetupPath(destroyList, out var path);
            SetupPathPoint(destroyList, path, new Vector3(0, 0, 1f));

            var forgotPath = false;
            Path pathForgotten = null;
            var basicEnemyScript = enemy.GetComponent<BasicEnemy>();
            basicEnemyScript.path = path.GetComponent<Path>();
            basicEnemyScript.ForgottenPath += path =>
            {
                forgotPath = true;
                pathForgotten = path;
            };

            yield return new WaitForSeconds(0.25f);

            Assert.IsTrue(forgotPath, "path has been forgotten");
            Assert.AreEqual(path, pathForgotten.gameObject, "the path passed in was forgotten");

            Teardown(destroyList);
        }
    }
}