using System.Collections;
using Model.Factories;
using MonoBehaviours.Combat;
using MonoBehaviours.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForTowerBuilder
{
    public class TowerBuilderFacts : ScenarioTest
    {
        private readonly PrefabSpawner _groundSpawner = new PrefabSpawner("Prefabs/Ground");
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
            Assert.AreEqual(1, towerBuilder.towerBuyButtons.Count,
                $"the tower count of {towerBuilder.towerBuyButtons.Count} is expected to be 1");
        }

        [UnityTest]
        public IEnumerator TowerBuilder_HelpsPlayer_WithAnIndicator()
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
            Assert.IsFalse(towerBuilder.indicator.gameObject.activeSelf, "The indicator should start off");

            buyButton.button.onClick.Invoke();
            yield return null;

            Assert.IsTrue(towerBuilder.indicator.gameObject.activeSelf, "The indicator should be on now");
        }

        [UnityTest]
        public IEnumerator TowerBuilder_Indicator_FollowsPlayerPointer()
        {
            var pointer = InputSystem.AddDevice<Pointer>();
            var instance = _prefabSpawner.Spawn();
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);
            CleanupAtEnd(_groundSpawner.Spawn());
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            buyButton.prefab = new GameObject();
            CleanupAtEnd(dummyBuyButton);

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();
            buyButton.button.onClick.Invoke();
            yield return null;
            Assert.AreEqual(Vector3.zero, towerBuilder.indicator.position, "indicator expected to start off at (0, 0, 0)");

            Move(pointer.position, new Vector2(0, 10));

            Assert.AreNotEqual(Vector3.zero, towerBuilder.indicator.position,
                "indicator expected to no longer be at (0, 0, 0)");
        }
    }
}
