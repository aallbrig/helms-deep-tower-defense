using System.Collections;
using Model.Factories;
using Model.Factories.Camera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPath
{
    public class PathFacts
    {
        private readonly TestCameraSpawner _testCameraSpawner = new TestCameraSpawner(new Vector3(0, 10, -10));
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Path");
    }
}