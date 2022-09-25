using UnityEngine;

namespace MonoBehaviours.Commerce
{
    public class Reward : MonoBehaviour, IRewardMoney
    {
        public int reward;

        public int RewardMoney => reward;
    }
}
