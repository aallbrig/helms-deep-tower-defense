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
        private readonly PrefabSpawner _basicTowerButton = new PrefabSpawner("Prefabs/UI/Buy Basic Tower Button");
        private readonly PrefabSpawner _groundSpawner = new PrefabSpawner("Prefabs/Ground");
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Tower Builder");
        [UnityTest]
        public IEnumerator TowerBuilder_UsesA_TowerBuilderComponent()
        {
            var instance = _prefabSpawner.Spawn();
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
            TestCameraLookAt(instance.transform);
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            buyButton.prefab = new GameObject();

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
            TestCameraLookAt(instance.transform);
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            buyButton.prefab = new GameObject();

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
            TestCameraLookAt(instance.transform);
            _groundSpawner.Spawn();
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            buyButton.prefab = new GameObject();

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();
            buyButton.button.onClick.Invoke();
            yield return null;
            Assert.AreEqual(Vector3.zero, towerBuilder.indicator.position, "indicator expected to start off at (0, 0, 0)");

            Move(pointer.position, new Vector2(0, 10));

            Assert.AreNotEqual(Vector3.zero, towerBuilder.indicator.position,
                "indicator expected to no longer be at (0, 0, 0)");
        }

        [UnityTest]
        public IEnumerator TowerBuilder_Indicator_IsAPreviewOfTheTower()
        {
            var pointer = InputSystem.AddDevice<Pointer>();
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            var dummyBuyButton = new GameObject();
            dummyBuyButton.AddComponent<Team>();
            var buyButton = dummyBuyButton.AddComponent<TowerBuyButton>();
            var buyButtonPrefab = new GameObject();
            buyButtonPrefab.name = "test tower";
            buyButton.prefab = buyButtonPrefab;

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();
            GameObject recordedTowerInstance = null;
            towerBuilder.PreviewIndicatorReplaced += towerInstance => recordedTowerInstance = towerInstance;
            buyButton.button.onClick.Invoke();
            yield return null;

            Move(pointer.position, new Vector2(0, 10));

            Assert.IsNotNull(recordedTowerInstance);
            Assert.AreEqual(buyButtonPrefab.name, recordedTowerInstance.name);
        }

        [UnityTest]
        public IEnumerator TowerBuilder_CannotPlace_TwoTowersAtSameLocation()
        {
            var pointer = InputSystem.AddDevice<Pointer>();
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            _groundSpawner.Spawn();
            var buyButtonInstance = _basicTowerButton.Spawn();
            var buyButton = buyButtonInstance.GetComponent<TowerBuyButton>();

            yield return null;
            var towerBuilder = instance.GetComponent<TowerBuilder>();
            var spawnCount = 0;
            towerBuilder.Spawned += _ => spawnCount++;

            // Try build two towers in the same location
            buyButton.button.onClick.Invoke();
            yield return null;
            Move(pointer.position, new Vector2(0, 10));
            PressAndRelease(pointer.press);
            yield return null;
            buyButton.button.onClick.Invoke();
            yield return null;
            // If you Move() to the same spot, no change occurs therefore no callbacks called
            Move(pointer.position, new Vector2(0, 0));
            Move(pointer.position, new Vector2(0, 10));
            PressAndRelease(pointer.press);
            yield return null;

            Assert.AreEqual(1, spawnCount);
        }

        [UnityTest]
        public IEnumerator TowerBuilder_CanParentSpawnedTowers_IfParentTransformSet()
        {
            var pointer = InputSystem.AddDevice<Pointer>();
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            var buyButtonInstance = _basicTowerButton.Spawn();
            var buyButton = buyButtonInstance.GetComponent<TowerBuyButton>();

            yield return null;
            var dummyParent = new GameObject();
            Assert.AreEqual(0, dummyParent.transform.childCount, "parent should start with 0 children");
            var towerBuilder = instance.GetComponent<TowerBuilder>();
            towerBuilder.parentTransform = dummyParent.transform;

            // Try build two towers in the same location
            buyButton.button.onClick.Invoke();
            yield return null;
            Move(pointer.position, new Vector2(0, 10));
            PressAndRelease(pointer.press);
            yield return null;

            Assert.AreEqual(1, dummyParent.transform.childCount, "parent should know about 1 child");
        }
    }
}
