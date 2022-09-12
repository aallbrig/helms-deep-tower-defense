using UnityEngine;

namespace ScriptableObjects
{
    public interface ITowerConfig
    {
        public float MaxHealth { get; }
        public float Range { get; }
    }

    [CreateAssetMenu(fileName = "new tower config", menuName = "Game/tower config", order = 0)]
    public class TowerConfig : ScriptableObject, ITowerConfig
    {
        public float maxHealth = 3.0f;
        public float range = 5.0f;

        public float MaxHealth => maxHealth;
        public float Range => range;
    }
}