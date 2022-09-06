using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "enemy configuration", menuName = "Game/new enemy configuration", order = 0)]
    public class EnemyConfiguration : ScriptableObject
    {
        public float moveSpeed = 3.5f;
    }
}