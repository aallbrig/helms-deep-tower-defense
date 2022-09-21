using System.Collections;
using System.Collections.Generic;
using Model.Factories.Camera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios
{
    public class ScenarioTest: InputTestFixture
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private GameObject _testCamera;
        private List<GameObject> _destroyAfterEachTest;

        protected void TestCameraLookAt(Transform target)
        {
            _testCamera.transform.LookAt(target);
        }
        protected void CleanupAtEnd(GameObject gameObject)
        {
            _destroyAfterEachTest.Add(gameObject);
        }

        [SetUp]
        protected void SetUp()
        {
            Debug.Log("ScenarioTest | SetUp | initializing brand new destroy list, for pure test");
            _destroyAfterEachTest = new List<GameObject>();
            Debug.Log("ScenarioTest | SetUp | creating test camera");
            _testCamera = _testCameraSpawner.Spawn();
            _destroyAfterEachTest.Add(_testCamera);
        }

        [TearDown]
        protected void TearDown()
        {
            Debug.Log("ScenarioTest | TearDown | destroying all game objects in destroy list");
            for (int i = _destroyAfterEachTest.Count - 1; i >= 0; i--)
                Object.Destroy(_destroyAfterEachTest[i]);
            _destroyAfterEachTest.Clear();
        }
    }
}