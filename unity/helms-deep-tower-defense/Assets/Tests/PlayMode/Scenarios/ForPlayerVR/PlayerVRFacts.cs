using System.Collections;
using Model.Controllers;
using Model.Factories;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPlayerVR
{
    public class PlayerVRFacts: ScenarioTest
    {
        private readonly ISpawner _instanceSpawner = new PrefabSpawner("Prefabs/XR Origin (Player)");

        [UnityTest]
        public IEnumerator PlayerVR_UsesA_InputController()
        {
            var instance = _instanceSpawner.Spawn();
            CleanupAtEnd(instance);
            TestCameraLookAt(instance.transform);
            yield return null;

            Assert.NotNull(instance.GetComponent<IVRInputController>());
        }
    }
}