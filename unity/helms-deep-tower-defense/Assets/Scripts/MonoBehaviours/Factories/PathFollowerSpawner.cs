using System;
using Model.Factories;
using Model.Locomotion;
using UnityEngine;

namespace MonoBehaviours.Factories
{
    public class PathFollowerSpawner : MonoBehaviour, ISpawner
    {
        public event Action<GameObject> Spawned;
        public GameObject pathFollower;
        public Path path;
        private Transform _transform;

        private void Start()
        {
            _transform = transform;
            path ??= FindObjectOfType<Path>();
            pathFollower ??= new GameObject();
        }

        [ContextMenu("Spawn")]
        public GameObject Spawn()
        {
            if (pathFollower != null)
            {
                var spawned = Instantiate(pathFollower, _transform.position, _transform.rotation);
                if (path != null && spawned.TryGetComponent<IFollowPath>(out var followPath))
                    followPath.path = path;
                Spawned?.Invoke(spawned);
                return spawned;
            }
            return null;
        }
    }
}
