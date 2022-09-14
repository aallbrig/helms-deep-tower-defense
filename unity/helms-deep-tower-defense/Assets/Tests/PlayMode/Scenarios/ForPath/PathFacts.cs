using System.Collections;
using Model.Factories;
using Model.Factories.Camera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForPath
{
    public class PathFacts: ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Path");
    }
}