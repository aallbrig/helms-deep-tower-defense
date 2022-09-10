using System.Collections;
using System.Collections.Generic;
using Model.Factories;
using MonoBehaviours;
using MonoBehaviours.Factories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPathFollowerSpawner
{
    public class PathFollowerSpawnerFacts
    {
        private void Setup(in List<GameObject> destroyList, out GameObject spawner)
        {
            var testCamera = new GameObject { transform = { position = new Vector3(0, 10, -10) } };
            testCamera.AddComponent<Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            destroyList.Add(testCamera);

            spawner = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Path Follower Spawner"));
            spawner.name = "System Under Test (sut)";
            destroyList.Add(spawner);

            testCamera.transform.LookAt(spawner.transform);
        }

        private void Teardown(List<GameObject> gameObjects)
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
                Object.Destroy(gameObjects[i]);
            gameObjects.Clear();
        }

        [UnityTest]
        public IEnumerator PathFollowerSpawner_SpawnsGameObjects_AndInjectsPath()
        {
            var spawned = false;
            var destroyList = new List<GameObject>();
            Setup(destroyList, out var spawner);
            var component = spawner.GetComponent<PathFollowerSpawner>();
            component.Spawned += _ => spawned = true;
            component.pathFollower = new GameObject();
            component.path = new GameObject().AddComponent<Path>();
            yield return null;

            spawner.GetComponent<ISpawner>().Spawn();

            Assert.IsTrue(spawned);

            Teardown(destroyList);
        }
    }
}