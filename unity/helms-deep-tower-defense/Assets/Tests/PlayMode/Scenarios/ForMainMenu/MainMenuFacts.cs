using System.Collections;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForMainMenu
{
    public class MainMenuFacts : ScenarioTest
    {
        private readonly PrefabSpawner _mainMenuSpawner = new PrefabSpawner("Prefabs/UI/Main Menu");
        [UnityTest]
        public IEnumerator MainMenu_Activates_WhenPauseInputPressed()
        {
            var mainMenu = _mainMenuSpawner.Spawn();
            CleanupAtEnd(mainMenu);
            TestCameraLookAt(mainMenu.transform);
            var mainMenuComponent = mainMenu.GetComponent<MainMenu>();
            yield return null;
            Assert.NotNull(mainMenu);
            Assert.NotNull(mainMenuComponent);
        }
    }
}
