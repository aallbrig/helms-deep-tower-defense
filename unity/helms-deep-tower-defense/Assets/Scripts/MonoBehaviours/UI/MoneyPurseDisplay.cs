using MonoBehaviours.Commerce;
using TMPro;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class MoneyPurseDisplay : MonoBehaviour
    {
        public MoneyPurse moneyPurse;
        public TextMeshProUGUI staticText;
        public TextMeshProUGUI dynamicText;

        private void Awake()
        {
            moneyPurse ??= FindObjectOfType<MoneyPurse>();
            if (moneyPurse) BindEvents();
        }
        private void BindEvents()
        {
            moneyPurse.NewCurrentMoneyPosted += OnNewCurrentMoneyPosted;
            moneyPurse.MoneyChanged += OnMoneyChanged;
        }
        private void OnMoneyChanged(int amountChangedBy)
        {
        }
        private void OnNewCurrentMoneyPosted(int currentMoney)
        {
            dynamicText.text = currentMoney.ToString();
        }
        public string Text()
        {
            return $"{staticText.text} {dynamicText.text}";
        }
    }
}
