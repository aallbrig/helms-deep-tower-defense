using Model.Factories;
using UnityEngine;

namespace MonoBehaviours.Factories.Spawners
{
    public class ParentSpawnedGameObject : MonoBehaviour
    {
        public Transform parent;
        private ISpawner _spawner;
        private void Start()
        {
            _spawner = GetComponent<ISpawner>();
            _spawner ??= GetComponentInChildren<ISpawner>();
            if (parent != null && _spawner != null)
            {
                _spawner.Spawned += go => go.transform.parent = parent.transform;
            }
        }
    }
}
