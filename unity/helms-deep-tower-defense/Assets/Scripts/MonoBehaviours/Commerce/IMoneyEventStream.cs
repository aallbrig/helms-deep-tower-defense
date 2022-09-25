using System;

namespace MonoBehaviours.Commerce
{
    public interface IMoneyEventStream
    {
        public event Action InsufficientFunds;
        public event Action<int> MoneyChanged;
        public event Action<int> NewCurrentMoneyPosted;
        public event Action<ICostMoney, bool> PriceChecked;
    }
}