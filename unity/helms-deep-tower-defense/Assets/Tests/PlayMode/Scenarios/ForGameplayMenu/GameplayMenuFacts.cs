using System.Collections;
using Model.Factories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForGameplayMenu
{
    public class GameplayMenuFacts : ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/UI/Gameplay Menu");

        [UnityTest]
        public IEnumerator GameplayMenu_UsesA_Canvas()
        {
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);

            yield return null;
            var canvasComponent = instance.GetComponent<Canvas>();
            Assert.NotNull(instance, "prefab instance needs to exist before being able to do any other testing");
            Assert.NotNull(canvasComponent);
        }
    }
}
