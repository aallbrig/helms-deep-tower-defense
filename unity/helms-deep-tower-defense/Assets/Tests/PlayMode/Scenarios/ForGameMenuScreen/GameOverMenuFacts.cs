using System.Collections;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForGameMenuScreen
{
    public class GameOverMenuFacts: ScenarioTest
    {
        private readonly PrefabSpawner _gameOverMenuSpawner = new PrefabSpawner("Prefabs/UI/Game Over Menu");
        private PrefabSpawner _gameRefereeSpawner = new PrefabSpawner("Prefabs/Systems/Game Referee");
        private PrefabSpawner _castleSpawner = new PrefabSpawner("Prefabs/Castle");
        [UnityTest]
        public IEnumerator GameOverMenu_Activates_WhenGameRefereeDeclaresGameOver()
        {
            var gameOverMenuGameObject = _gameOverMenuSpawner.Spawn();
            var gameOverMenu = gameOverMenuGameObject.GetComponent<GameOverMenu>();
            var gameOverMenuActivated = false;
            gameOverMenu.GameOverMenuIsActivated += () => gameOverMenuActivated = true;
            CleanupAtEnd(gameOverMenuGameObject);
            CleanupAtEnd(_gameRefereeSpawner.Spawn());
            var castleGameObject = _castleSpawner.Spawn();
            CleanupAtEnd(castleGameObject);
            TestCameraLookAt(castleGameObject.transform);
            yield return null;
            castleGameObject.GetComponent<MonoBehaviours.Castle>().Kill();

            Assert.IsTrue(gameOverMenuActivated);
        }
    }
}
