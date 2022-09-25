using UnityEngine;

namespace MonoBehaviours.Commerce
{
    public class Cost : MonoBehaviour, ICostMoney
    {
        public int price;

        public int Price => price;
    }
}
