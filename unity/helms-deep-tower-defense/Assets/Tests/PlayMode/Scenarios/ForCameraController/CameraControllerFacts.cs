using System.Collections;
using MonoBehaviours.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForCameraController
{
    public class CameraControllerFacts : ScenarioTest
    {
        [UnityTest]
        public IEnumerator CameraController_ZoomsIn_UsingTouchControls()
        {
            var sut = new GameObject().AddComponent<CameraController>();
            InputSystem.AddDevice<Touchscreen>();
            yield return null;
            BeginTouch(1, new Vector2(0, 0));
            BeginTouch(2, new Vector2(0, 0));
            yield return null;
        }
    }
}
