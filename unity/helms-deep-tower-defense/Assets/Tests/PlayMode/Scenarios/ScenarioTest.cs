using System.Collections;
using Model.Factories.Camera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios
{
    public class ScenarioTest : InputTestFixture
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private GameObject _testCamera;

        protected void TestCameraLookAt(Transform target) => _testCamera.transform.LookAt(target);

        [SetUp]
        protected void SetUp()
        {
            Debug.Log("ScenarioTest | SetUp | creating test camera");
            _testCamera = _testCameraSpawner.Spawn();
        }

        [UnityTearDown]
        protected IEnumerator UnityTearDown()
        {
            var remainingGameObjects = Object.FindObjectsOfType<GameObject>(true);
            Debug.Log($"ScenarioTest | TearDown | game objects in scene: {remainingGameObjects.Length}");
            foreach (var remainingGameObject in remainingGameObjects)
                Object.Destroy(remainingGameObject);
            yield return null; // allow end of frame, so the game objects really get cleaned up

            if (SceneManager.sceneCount > 1)
                yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            for (var i = SceneManager.sceneCount - 1; i > 0; i--)
            {
                Debug.Log("ScenarioTest | UnityTearDown | Unloading added scenes");
                yield return SceneManager.UnloadSceneAsync(i);
            }
        }
    }
}
