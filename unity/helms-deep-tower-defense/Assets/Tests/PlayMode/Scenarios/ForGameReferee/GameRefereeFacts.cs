using System.Collections;
using Model.Factories;
using MonoBehaviours.Systems;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForGameReferee
{
    public class GameRefereeFacts : ScenarioTest
    {
        private readonly PrefabSpawner _castleSpawner = new PrefabSpawner("Prefabs/Castle");
        private readonly PrefabSpawner _refereeSpawner = new PrefabSpawner("Prefabs/Systems/Game Referee");
        private readonly PrefabSpawner _waveSpawnerSpawner = new PrefabSpawner("Prefabs/Spawners/Test Wave");
        [UnityTest]
        public IEnumerator GameReferee_DeclaresGameLost_WhenAllCastlesAreDestroyed()
        {
            var referee = _refereeSpawner.Spawn();
            TestCameraLookAt(referee.transform);
            var refereeComponent = referee.GetComponent<GameReferee>();
            var allCastlesDestroyedEventCalled = false;
            refereeComponent.AllCastlesDestroyed += () => allCastlesDestroyedEventCalled = true;
            var gameIsOverEventCalled = false;
            refereeComponent.GameIsOver += () => gameIsOverEventCalled = true;
            var castleGameObject1 = _castleSpawner.Spawn();
            var castleGameObject2 = _castleSpawner.Spawn();
            var castleGameObject3 = _castleSpawner.Spawn();
            yield return null;

            castleGameObject1.GetComponent<MonoBehaviours.Castle>().Kill();
            castleGameObject2.GetComponent<MonoBehaviours.Castle>().Kill();
            castleGameObject3.GetComponent<MonoBehaviours.Castle>().Kill();

            Assert.IsTrue(allCastlesDestroyedEventCalled);
            Assert.IsTrue(gameIsOverEventCalled);
        }

        [UnityTest]
        public IEnumerator GameReferee_TracksInactiveCastles_InTheScene()
        {
            var referee = _refereeSpawner.Spawn();
            TestCameraLookAt(referee.transform);
            var refereeComponent = referee.GetComponent<GameReferee>();
            _castleSpawner.Spawn();
            _castleSpawner.Spawn();
            var inactivateCastleGameObject = _castleSpawner.Spawn();
            inactivateCastleGameObject.SetActive(false);
            yield return null;

            Assert.AreEqual(3, refereeComponent.castlesAlive);
        }

        [UnityTest]
        public IEnumerator GameReferee_TracksInactiveSpawners_InTheScene()
        {
            var referee = _refereeSpawner.Spawn();
            TestCameraLookAt(referee.transform);
            var refereeComponent = referee.GetComponent<GameReferee>();
            _waveSpawnerSpawner.Spawn();
            _waveSpawnerSpawner.Spawn();
            var inactivateWaveSpawnerGameObject = _waveSpawnerSpawner.Spawn();
            inactivateWaveSpawnerGameObject.SetActive(false);
            yield return null;

            Assert.AreEqual(3, refereeComponent.waves);
        }

        [UnityTest]
        public IEnumerator GameReferee_KnowsAbout_AllCastlesInScene()
        {
            var referee = _refereeSpawner.Spawn();
            TestCameraLookAt(referee.transform);
            var refereeComponent = referee.GetComponent<GameReferee>();
            var castleRegisteredCounter = 0;
            refereeComponent.CastleRegistered += _ => castleRegisteredCounter++;
            _castleSpawner.Spawn();
            _castleSpawner.Spawn();
            _castleSpawner.Spawn();
            yield return null;

            Assert.AreEqual(3, castleRegisteredCounter, "expects the ref to declare how many castles its tracking");
        }
    }
}
