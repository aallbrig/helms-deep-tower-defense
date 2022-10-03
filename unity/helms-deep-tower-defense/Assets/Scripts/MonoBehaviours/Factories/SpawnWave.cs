using System;
using Model.Factories;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Factories
{
    public class SpawnWave : MonoBehaviour, ISpawner
    {
        public event Action WaveCompleted;
        public WaveConfiguration config;
        private float _timeOfLastSpawn;
        private int _currentCount;
        private bool _waveComplete = false;
        private WaveConfiguration _configCopy;

        private void Start()
        {
            ResetWaveState();
        }

        private void Update()
        {
            if (_currentCount >= config.spawnCount) return;

            if (Time.time - _timeOfLastSpawn >= config.delayInSeconds)
            {
                _timeOfLastSpawn = Time.time;
                WaveSpawn();
                _currentCount++;
                if (_currentCount >= config.spawnCount)
                {
                    _waveComplete = true;
                    WaveCompleted?.Invoke();
                }
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

        public event Action<GameObject> Spawned;

        public void WaveSpawn()
        {
            config.enemies.ForEach(prefab =>
            {
                Spawned?.Invoke(Instantiate(prefab));
            });
        }
    }
}
