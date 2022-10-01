using System.Collections;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode.Scenarios.ForMainMenu
{
    public class MainMenuFacts : ScenarioTest
    {
        private readonly PrefabSpawner _mainMenuSpawner = new PrefabSpawner("Prefabs/UI/Main Menu");

        [UnityTest]
        public IEnumerator MainMenu_Activates_OnStart()
        {
            var mainMenu = _mainMenuSpawner.Spawn();
            TestCameraLookAt(mainMenu.transform);
            var mainMenuComponent = mainMenu.GetComponent<MainMenu>();
            var activatedEventCalled = false;
            mainMenuComponent.MainMenuActivated += () => activatedEventCalled = true;

            yield return null;

            Assert.NotNull(mainMenu);
            Assert.NotNull(mainMenuComponent);
            Assert.IsTrue(activatedEventCalled);
        }

        [UnityTest]
        public IEnumerator MainMenu_LoadsNewGame_OnNewGameButtonPress()
        {
            var mainMenu = _mainMenuSpawner.Spawn();
            TestCameraLookAt(mainMenu.transform);
            yield return null;
            Assert.AreEqual(1, SceneManager.sceneCount);
            GameObject.Find("New Game Button").GetComponent<Button>().onClick.Invoke();
            Assert.AreEqual(2, SceneManager.sceneCount);
            Assert.AreEqual("Game", SceneManager.GetSceneAt(1).name);
        }
    }
}
