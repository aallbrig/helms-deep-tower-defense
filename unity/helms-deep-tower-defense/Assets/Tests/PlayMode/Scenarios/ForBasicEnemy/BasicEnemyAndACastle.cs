using System.Collections;
using System.Collections.Generic;
using MonoBehaviours.AI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForBasicEnemy
{
    public class BasicEnemyAndACastle
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
        private void SetupCastle(in List<GameObject> destroyList, out GameObject castle)
        {
            castle = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Castle"));
            castle.name = $"{castle.name} (test)";
            destroyList.Add(castle);
        }

        private void Teardown(List<GameObject> gameObjectsToDestroy)
        {
            for (var i = gameObjectsToDestroy.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjectsToDestroy[i]);
            gameObjectsToDestroy.Clear();
        }

        [UnityTest]
        public IEnumerator BasicEnemy_CanAttack_ACastle()
        {
            var castleAttacked = false;
            var destroyList = new List<GameObject>();
            SetupCamera(destroyList, out var camera);
            SetupEnemy(destroyList, out var enemy);
            camera.transform.LookAt(enemy.transform);
            SetupCastle(destroyList, out var castle);
            castle.GetComponent<MonoBehaviours.Castle>().Damaged += _ => castleAttacked = true;
            castle.transform.position = enemy.transform.position;

            yield return null;
            yield return null;

            Assert.IsTrue(castleAttacked);

            Teardown(destroyList);
            yield return null;
        }
    }
}