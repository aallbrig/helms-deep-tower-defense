using System.Collections;
using Model.Factories;
using MonoBehaviours.Factories;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForTowerBuilder
{
    public class TowerBuilderFacts : ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Tower Builder");
        [UnityTest]
        public IEnumerator TowerBuilder_UsesA_TowerBuilderComponent()
        {
            var instance = _prefabSpawner.Spawn();
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();

            Assert.NotNull(instance, "instance of prefab needs to exist");
            Assert.NotNull(towerBuilder, "tower builder component needs to exist");
        }
    }
}