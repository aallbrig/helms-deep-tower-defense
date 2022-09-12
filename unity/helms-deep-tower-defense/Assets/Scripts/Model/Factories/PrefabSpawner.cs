using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Model.Factories
{
    public class PrefabSpawner: ISpawner
    {
        public event Action<GameObject> Spawned;
        private readonly string _resourcePath;

        public PrefabSpawner(string resourcePath) => _resourcePath = resourcePath;

        public GameObject Spawn()
        {
            var prefabInstance = Object.Instantiate(Resources.Load<GameObject>(_resourcePath));
            Spawned?.Invoke(prefabInstance);
            return prefabInstance;
        }
    }
}