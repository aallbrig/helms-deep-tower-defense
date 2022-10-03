using System;
using UnityEngine;

namespace Model.Factories
{
    public interface ISpawner
    {
        public event Action<GameObject> Spawned;
    }
}
