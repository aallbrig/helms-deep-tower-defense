using System.Collections;
using Model.Factories;
using MonoBehaviours.Combat;
using MonoBehaviours.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
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

        [UnityTest]
        public IEnumerator TowerBuilder_DetectsAll_TowerBuyButtons()
        {
            var instance = _prefabSpawner.Spawn();
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            buyButton.prefab = new GameObject();
            CleanupAtEnd(dummyBuyButton);

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();

            Assert.NotNull(instance, "instance of prefab needs to exist");
            Assert.NotNull(towerBuilder, "tower builder component needs to exist");
            Assert.AreEqual(1, towerBuilder.towerBuyButtons.Count, $"the tower count of {towerBuilder.towerBuyButtons.Count} is expected to be 1");
        }
    }
}