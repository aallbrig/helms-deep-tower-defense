using System.Collections;
using Model.Combat;
using Model.Factories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForAttackPointsTrigger
{
    public class AttackPointsFacts:ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Castle/Attack Points");
        private GameObject _prefabInstance;
        [SetUp]
        public new void SetUp()
        {
            Debug.Log("AttackPointFacts | Setup run");
            _prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(_prefabInstance);

            TestCameraLookAt(_prefabInstance.transform);
        }
        [UnityTest]
        public IEnumerator AttackPoints_API_CanAssignAttackPoint()
        {
            yield return null;
            var assigner = _prefabInstance.GetComponent<IAssignAttackPoints>();

            Assert.NotNull(assigner, "no component on prefab instance implements IAssignAttackPoints");
        }
    }
}