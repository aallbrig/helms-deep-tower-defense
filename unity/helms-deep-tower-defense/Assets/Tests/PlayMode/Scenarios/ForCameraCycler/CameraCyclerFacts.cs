using System.Collections;
using Cinemachine;
using Model.Factories;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode.Scenarios.ForCameraCycler
{
    public class CameraCyclerFacts : ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/UI/Camera Cycler");

        [UnityTest]
        public IEnumerator CameraCycler_DetectsAllCinemachineVirtualCameras_InScene()
        {
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            var cyclerComponent = instance.GetComponent<CameraCycler>();

            new GameObject().AddComponent<CinemachineVirtualCamera>();
            new GameObject().AddComponent<CinemachineVirtualCamera>();
            new GameObject().AddComponent<CinemachineVirtualCamera>();
            yield return null;

            Assert.AreEqual(3, cyclerComponent.cameras.Length);
        }

        [UnityTest]
        public IEnumerator CameraCycler_CyclesThroughVirtualCameras_ForwardAndBackward()
        {
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            var forwardsCalled = false;
            var backwardsCalled = false;
            var cyclerComponent = instance.GetComponent<CameraCycler>();
            cyclerComponent.CycledForwards += () => forwardsCalled = true;
            cyclerComponent.CycledBackwards += () => backwardsCalled = true;
            var forwardButton = instance.transform.Find("Cycle Forward Button").GetComponent<Button>();
            var backButton = instance.transform.Find("Cycle Back Button").GetComponent<Button>();
            Assert.NotNull(forwardButton);
            Assert.NotNull(backButton);

            new GameObject().AddComponent<CinemachineVirtualCamera>();
            new GameObject().AddComponent<CinemachineVirtualCamera>();
            new GameObject().AddComponent<CinemachineVirtualCamera>();
            yield return null;

            forwardButton.onClick.Invoke();
            backButton.onClick.Invoke();

            Assert.IsTrue(forwardsCalled, "forwards not called");
            Assert.IsTrue(backwardsCalled, "backwards not called");
        }

        [UnityTest]
        public IEnumerator CameraCycler_CyclesThroughVirtualCameras_Correctly()
        {
            var instance = _prefabSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            var forwardButton = instance.transform.Find("Cycle Forward Button").GetComponent<Button>();
            var backButton = instance.transform.Find("Cycle Back Button").GetComponent<Button>();
            Assert.NotNull(forwardButton);
            Assert.NotNull(backButton);

            var camera1 = new GameObject { name = "test camera 1" }.AddComponent<CinemachineVirtualCamera>().gameObject;
            var camera2 = new GameObject { name = "test camera 2" }.AddComponent<CinemachineVirtualCamera>().gameObject;
            var camera3 = new GameObject { name = "test camera 3" }.AddComponent<CinemachineVirtualCamera>().gameObject;
            camera1.GetComponent<CinemachineVirtualCamera>().Priority = 30;
            camera2.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            camera3.GetComponent<CinemachineVirtualCamera>().Priority = 10;
            yield return null;

            Assert.IsTrue(camera1.gameObject.activeSelf);
            Assert.IsFalse(camera2.gameObject.activeSelf);
            Assert.IsFalse(camera3.gameObject.activeSelf);
            forwardButton.onClick.Invoke();
            Assert.IsFalse(camera1.gameObject.activeSelf);
            Assert.IsTrue(camera2.gameObject.activeSelf);
            Assert.IsFalse(camera3.gameObject.activeSelf);
            forwardButton.onClick.Invoke();
            Assert.IsFalse(camera1.gameObject.activeSelf);
            Assert.IsFalse(camera2.gameObject.activeSelf);
            Assert.IsTrue(camera3.gameObject.activeSelf);
            backButton.onClick.Invoke();
            Assert.IsFalse(camera1.gameObject.activeSelf);
            Assert.IsTrue(camera2.gameObject.activeSelf);
            Assert.IsFalse(camera3.gameObject.activeSelf);
            backButton.onClick.Invoke();
            Assert.IsTrue(camera1.gameObject.activeSelf);
            Assert.IsFalse(camera2.gameObject.activeSelf);
            Assert.IsFalse(camera3.gameObject.activeSelf);
        }
    }
}
