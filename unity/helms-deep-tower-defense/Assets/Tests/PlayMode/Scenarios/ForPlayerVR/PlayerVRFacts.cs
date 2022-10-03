using System.Collections;
using Model.Controllers;
using Model.Factories;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPlayerVR
{
    public class PlayerVRFacts: ScenarioTest
    {
        private readonly PrefabSpawner _instanceSpawner = new PrefabSpawner("Prefabs/XR Origin (Player)");

        public IEnumerator PlayerVR_UsesA_InputController()
        {
            var instance = _instanceSpawner.Spawn();
            TestCameraLookAt(instance.transform);
            yield return null;

            Assert.NotNull(instance.GetComponent<IVRInputController>());
        }
    }
}
