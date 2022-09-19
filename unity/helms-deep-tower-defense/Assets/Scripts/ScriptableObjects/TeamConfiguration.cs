using UnityEngine;

namespace ScriptableObjects
{
    public interface ITeamConfig
    {
        public LayerMask allies { get; }

        public LayerMask enemies { get; }
    }

    [CreateAssetMenu(fileName = "new team config", menuName = "Game/Team Configuration", order = 0)]
    public class TeamConfiguration : ScriptableObject, ITeamConfig
    {
        public LayerMask alliesLayerMask;
        public LayerMask enemiesLayerMask;

        public LayerMask allies => alliesLayerMask;

        public LayerMask enemies => enemiesLayerMask;

        public static bool IsInLayerMask(GameObject other, LayerMask layerMask) =>
            layerMask == (layerMask | (1 << other.layer));
    }
}