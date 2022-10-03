using System.Collections.Generic;
using Model.Factories;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "new wave config", menuName = "Game/Wave Config", order = 0)]
    public class WaveConfiguration : ScriptableObject, IWaveConfig
    {
        public int spawnCount = 1;
        public float delayInSeconds = 1f;
        public List<GameObject> enemies;

        public int SpawnCount => spawnCount;
        public float DelayInSeconds => delayInSeconds;
        public List<GameObject> Prefabs => enemies;
    }
}
