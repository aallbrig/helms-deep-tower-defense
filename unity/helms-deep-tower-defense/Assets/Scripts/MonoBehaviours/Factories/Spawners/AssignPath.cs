using Model.Factories;
using Model.Locomotion;
using UnityEngine;

namespace MonoBehaviours.Factories.Spawners
{
    public class AssignPath : MonoBehaviour
    {
        public Path pathToAssign;
        private ISpawner _spawner;
        private void Awake()
        {
            if (pathToAssign == null)
            {
                // find all available paths
                // choose the closest path
                Path closestPath = null;
                var closestDistance = 100000f;
                foreach (var path in FindObjectsOfType<Path>())
                {
                    var distance = Vector3.Distance(transform.position, path.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPath = path;
                    }
                }
                if (closestPath != null) pathToAssign = closestPath;
            }
            _spawner = GetComponent<ISpawner>();
            _spawner ??= GetComponentInChildren<ISpawner>();
        }
        private void Start() => _spawner.Spawned += OnSpawn;
        private void OnSpawn(GameObject spawnedGameObject)
        {
            if (pathToAssign && spawnedGameObject.TryGetComponent<IFollowPath>(out var pathFollower))
                pathFollower.path = pathToAssign;
        }
    }
}
