using System;
using UnityEngine;

namespace MonoBehaviours.Commerce
{

    public class MoneyPurse : MonoBehaviour, IMoneyEventStream
    {
        public int currentMoney;
        public event Action InsufficientFunds;
        public event Action<int> MoneyChanged;
        public event Action<int> NewCurrentMoneyPosted;
        public event Action<ICostMoney, bool> PriceChecked;
        private void Start()
        {
            MoneyChanged?.Invoke(currentMoney);
            NewCurrentMoneyPosted?.Invoke(currentMoney);
        }
        private bool CanAfford(ICostMoney costable)
        {
            var canAffordCost = costable.Price <= currentMoney;
            return canAffordCost;
        }
        public bool PriceCheck(ICostMoney purchaseable)
        {
            var canAfford = CanAfford(purchaseable);
            PriceChecked?.Invoke(purchaseable, canAfford);
            if (canAfford == false) InsufficientFunds?.Invoke();
            return canAfford;
        }
        public void Purchase(ICostMoney purchaseable)
        {
            if (CanAfford(purchaseable)) ChangeMoney(-purchaseable.Price);
            else InsufficientFunds?.Invoke();
        }
        public void AddReward(IRewardMoney reward)
        {
            ChangeMoney(reward.RewardMoney);
        }
        private void ChangeMoney(int moneyChange)
        {
            currentMoney += moneyChange;
            MoneyChanged?.Invoke(moneyChange);
            NewCurrentMoneyPosted?.Invoke(currentMoney);
        }
    }
}
