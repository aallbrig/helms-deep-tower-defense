using System.Collections.Generic;
using UnityEngine;

namespace Model.Factories
{
    public interface IWaveConfig
    {
        public int SpawnCount { get; }
        public float DelayInSeconds { get; }
        public List<GameObject> Prefabs { get; }
    }
}
