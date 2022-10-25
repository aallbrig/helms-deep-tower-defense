using System.Collections;
using MonoBehaviours.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Tests.PlayMode.Scenarios.ForCameraController
{
    public class CameraControllerFacts : ScenarioTest
    {
        [UnityTest]
        public IEnumerator CameraController_ZoomsIn_UsingTouchControls()
        {
            var sut = new GameObject().AddComponent<CameraController>();
            var device = InputSystem.AddDevice<Touchscreen>();
            yield return null;
            BeginTouch(1, new Vector2(0, 0));
            BeginTouch(2, new Vector2(0, 0));
            yield return null;
            SetTouch(1, TouchPhase.Moved, new Vector2(0, 1));
            SetTouch(2, TouchPhase.Moved, new Vector2(0, -1));
            yield return null;
            SetTouch(1, TouchPhase.Moved, new Vector2(0, 10));
            SetTouch(2, TouchPhase.Moved, new Vector2(0, -10));
            yield return null;
            EndTouch(1, new Vector2(0, 10));
            EndTouch(2, new Vector2(0, -10));
            yield return null;
            InputSystem.RemoveDevice(device);
            yield return null;
        }
    }
}
