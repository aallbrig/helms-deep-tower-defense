using System;
using Model.Factories;
using MonoBehaviours.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Factories
{
    public class TowerBuilder : MonoBehaviour, ISpawner
    {
        public TowerConfiguration activeTowerConfiguration;
        public TeamConfiguration activeTeamConfiguration;

        public event Action<GameObject> Spawned;

        public GameObject Spawn()
        {
            var newTower = new GameObject();
            if (newTower.TryGetComponent<Team>(out var team)) {}
            Spawned?.Invoke(newTower);
            return newTower;
        }
    }
}