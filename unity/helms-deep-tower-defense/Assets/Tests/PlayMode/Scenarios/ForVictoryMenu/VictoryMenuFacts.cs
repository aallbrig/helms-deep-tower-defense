using System;
using System.Collections;
using System.Linq;
using Model.Combat;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode.Scenarios.ForVictoryMenu
{
    public class VictoryMenuFacts: ScenarioTest
    {
        private readonly PrefabSpawner _victoryMenuSpawner = new PrefabSpawner("Prefabs/UI/Victory Menu");
        private readonly PrefabSpawner _refereeSpawner = new PrefabSpawner("Prefabs/Systems/Game Referee");
        private readonly PrefabSpawner _pathSpawner = new PrefabSpawner("Prefabs/Paths/Test Path");
        private readonly PrefabSpawner _pathFollowerSpawnerSpawner = new PrefabSpawner("Prefabs/Spawners/Test Wave");

        [UnityTest]
        public IEnumerator VictoryMenuFactsWithEnumeratorPasses()
        {
            var victoryMenu = _victoryMenuSpawner.Spawn();
            var victoryMenuComponent = victoryMenu.GetComponent<VictoryMenu>();
            var victoryMenuActivated = false;
            victoryMenuComponent.VictoryMenuActivated += () => victoryMenuActivated = true;
            CleanupAtEnd(victoryMenu);
            CleanupAtEnd(_refereeSpawner.Spawn());
            CleanupAtEnd(_pathSpawner.Spawn());
            CleanupAtEnd(_pathFollowerSpawnerSpawner.Spawn());
            yield return null;

            foreach (var killable in Object.FindObjectsOfType<MonoBehaviour>().OfType<IKillable>())
                killable.Kill();

            Assert.IsTrue(victoryMenuActivated);
        }
    }
}
