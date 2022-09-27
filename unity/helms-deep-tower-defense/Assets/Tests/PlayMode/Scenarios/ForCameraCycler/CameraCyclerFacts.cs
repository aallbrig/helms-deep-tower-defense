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
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);
            var cyclerComponent = instance.GetComponent<CameraCycler>();

            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            yield return null;

            Assert.AreEqual(3, cyclerComponent.cameras.Length);
        }

        [UnityTest]
        public IEnumerator CameraCycler_CyclesThroughVirtualCameras_ForwardAndBackward()
        {
            var instance = _prefabSpawner.Spawn();
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);
            var forwardsCalled = false;
            var backwardsCalled = false;
            var cyclerComponent = instance.GetComponent<CameraCycler>();
            cyclerComponent.CycledForwards += () => forwardsCalled = true;
            cyclerComponent.CycledBackwards += () => backwardsCalled = true;
            var forwardButton = instance.transform.Find("Cycle Back Button").GetComponent<Button>();
            var backButton = instance.transform.Find("Cycle Forward Button").GetComponent<Button>();
            Assert.NotNull(forwardButton);
            Assert.NotNull(backButton);

            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            CleanupAtEnd(new GameObject().AddComponent<CinemachineVirtualCamera>().gameObject);
            yield return null;

            forwardButton.onClick.Invoke();
            backButton.onClick.Invoke();

            Assert.IsTrue(forwardsCalled, "forwards not called");
            Assert.IsTrue(backwardsCalled, "backwards not called");
        }
    }
}
