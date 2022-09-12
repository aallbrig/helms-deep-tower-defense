using System.Collections;
using System.Collections.Generic;
using Model.Factories;
using Model.Factories.Camera;
using MonoBehaviours;
using MonoBehaviours.Factories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPathFollowerSpawner
{
    public class PathFollowerSpawnerFacts
    {
        private readonly TestCamera _testCameraSpawner = new TestCamera(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Path Follower Spawner");

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
            var spawner = _prefabSpawner.Spawn();
            destroyList.Add(spawner);
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            testCamera.transform.LookAt(spawner.transform);
            var component = spawner.GetComponent<PathFollowerSpawner>();
            component.Spawned += _ => spawned = true;
            component.pathFollower = new GameObject();
            component.path = new GameObject().AddComponent<Path>();
            yield return null;

            spawner.GetComponent<ISpawner>().Spawn();

            Assert.IsTrue(spawned);

            Teardown(destroyList);
        }

        [UnityTest]
        public IEnumerator PathFollowerSpawner_SpawnsGameObjects_BasedOnWaveConfig()
        {
            var spawned = false;
            var destroyList = new List<GameObject>();
            var testCamera = _testCameraSpawner.Spawn();
            destroyList.Add(testCamera);
            var spawner = _prefabSpawner.Spawn();
            destroyList.Add(spawner);
            testCamera.transform.LookAt(spawner.transform);
            var spawnerComponent = spawner.GetComponent<PathFollowerSpawner>();
            spawnerComponent.Spawned += _ => spawned = true;
            spawnerComponent.pathFollower = new GameObject();
            spawnerComponent.path = new GameObject().AddComponent<Path>();
            yield return null;

            spawnerComponent.GetComponent<ISpawner>().Spawn();

            Assert.IsTrue(spawned);

            Teardown(destroyList);
        }
    }
}