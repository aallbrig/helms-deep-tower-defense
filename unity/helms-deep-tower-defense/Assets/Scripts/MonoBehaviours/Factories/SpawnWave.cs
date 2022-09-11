using System;
using Model.Factories;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Factories
{
    public class SpawnWave : MonoBehaviour
    {
        public WaveConfiguration config;
        public PathFollowerSpawner spawner;
        private ISpawner _spawner;
        private float _timeOfLastSpawn;
        private int _currentCount;
        private WaveConfiguration _configCopy;

        private void Start()
        {
            if (spawner) _spawner = spawner;
            _spawner ??= GetComponent<ISpawner>();
            ResetWaveState();
        }

        private void Update()
        {
            if (_spawner == null || _currentCount > config.spawnCount) return;

            if (Time.time - _timeOfLastSpawn >= config.delayInSeconds)
            {
                _timeOfLastSpawn = Time.time;
                _spawner.Spawn();
                _currentCount++;
            }
        }

        private void OnValidate()
        {
            if (config != _configCopy)
            {
                _configCopy = config;
                ResetWaveState();
            }
        }

        private void ResetWaveState()
        {
            _timeOfLastSpawn = Time.time - config.delayInSeconds;
            _currentCount = 0;
        }
    }
}