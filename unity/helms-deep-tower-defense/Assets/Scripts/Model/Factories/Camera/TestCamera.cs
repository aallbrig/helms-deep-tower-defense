using System;
using UnityEngine;

namespace Model.Factories.Camera
{
    public class TestCamera: ISpawner
    {
        public event Action<GameObject> Spawned;
        private readonly Vector3 _position;

        public TestCamera(Vector3 position) => _position = position;

        public GameObject Spawn()
        {
            var testCamera = new GameObject { transform = { position = _position } };
            testCamera.AddComponent<UnityEngine.Camera>();
            testCamera.name = "Test Scenario Camera";
            testCamera.tag = "MainCamera";
            Spawned?.Invoke(testCamera);
            return testCamera;
        }
    }
}