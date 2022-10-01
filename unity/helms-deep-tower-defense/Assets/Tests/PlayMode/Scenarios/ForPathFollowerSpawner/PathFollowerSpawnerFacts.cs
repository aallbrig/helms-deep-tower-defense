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
    public class PathFollowerSpawnerFacts: ScenarioTest
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Spawners/Path Follower Spawner");

        [UnityTest]
        public IEnumerator PathFollowerSpawner_SpawnsGameObjects_AndInjectsPath()
        {
            var spawned = false;
            var spawner = _prefabSpawner.Spawn();
            TestCameraLookAt(spawner.transform);
            var component = spawner.GetComponent<PathFollowerSpawner>();
            component.Spawned += _ => spawned = true;
            component.pathFollower = new GameObject();
            component.path = new GameObject().AddComponent<Path>();
            yield return null;

            spawner.GetComponent<ISpawner>().Spawn();

            Assert.IsTrue(spawned);
        }

        [UnityTest]
        public IEnumerator PathFollowerSpawner_SpawnsGameObjects_BasedOnWaveConfig()
        {
            var spawned = false;
            var spawner = _prefabSpawner.Spawn();
            TestCameraLookAt(spawner.transform);
            var spawnerComponent = spawner.GetComponent<PathFollowerSpawner>();
            spawnerComponent.Spawned += _ => spawned = true;
            spawnerComponent.pathFollower = new GameObject();
            spawnerComponent.path = new GameObject().AddComponent<Path>();
            yield return null;

            spawnerComponent.GetComponent<ISpawner>().Spawn();

            Assert.IsTrue(spawned);
        }
    }
}
