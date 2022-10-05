using System.Collections;
using System.Linq;
using Model.Combat;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForVictoryMenu
{
    public class VictoryMenuFacts : ScenarioTest
    {
        private readonly PrefabSpawner _pathFollowerSpawnerSpawner = new PrefabSpawner("Prefabs/Spawners/Test Wave");
        private readonly PrefabSpawner _pathSpawner = new PrefabSpawner("Prefabs/Paths/Test Path");
        private readonly PrefabSpawner _refereeSpawner = new PrefabSpawner("Prefabs/Systems/Game Referee");
        private readonly PrefabSpawner _victoryMenuSpawner = new PrefabSpawner("Prefabs/UI/Victory Menu");

        [UnityTest]
        public IEnumerator VictoryMenu_Activates_OnWinCondition()
        {
            var victoryMenu = _victoryMenuSpawner.Spawn();
            var victoryMenuComponent = victoryMenu.GetComponent<VictoryMenu>();
            var victoryMenuActivated = false;
            victoryMenuComponent.VictoryMenuActivated += () => victoryMenuActivated = true;
            _refereeSpawner.Spawn();
            _pathSpawner.Spawn();
            _pathFollowerSpawnerSpawner.Spawn();
            yield return null;
            Time.timeScale = 10.0f;
            yield return new WaitForSeconds(1.1f);
            Time.timeScale = 1.0f;

            foreach (var killable in Object.FindObjectsOfType<MonoBehaviour>().OfType<IKillable>())
                killable.Kill();

            Assert.IsTrue(victoryMenuActivated);
        }
    }
}
