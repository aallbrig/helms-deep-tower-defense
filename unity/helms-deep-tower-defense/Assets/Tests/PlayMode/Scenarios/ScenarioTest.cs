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

        [TearDown]
        protected new void TearDown()
        {
            var remainingGameObjects = Object.FindObjectsOfType<GameObject>();
            Debug.Log($"ScenarioTest | TearDown | game objects in scene: {remainingGameObjects.Length}");
            foreach (var remainingGameObject in remainingGameObjects)
                Object.Destroy(remainingGameObject);
        }

        [UnityTearDown]
        protected IEnumerator UnityTearDown()
        {
            for (int i = SceneManager.sceneCount - 1; i > 0; i--)
            {
                Debug.Log("ScenarioTest | UnityTearDown | Unloading added scenes");
                yield return SceneManager.UnloadSceneAsync(i);
            }
        }

    }
}
