using System.Collections;
using Model.Factories;
using MonoBehaviours.Commerce;
using MonoBehaviours.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForMoneyPurse
{
    public class MoneyPurseFacts: ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Systems/Money Purse");
        private readonly PrefabSpawner _displayGuiSpawner = new PrefabSpawner("Prefabs/UI/Money Display");
        [UnityTest]
        public IEnumerator MoneyPurse_Uses_Components()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            yield return null;

            Assert.IsTrue(prefabInstance.TryGetComponent<MoneyPurse>(out _));
        }

        [UnityTest]
        public IEnumerator MoneyPurse_BroadcastsCurrentAmount_Initially()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            var moneyPurseComponent = prefabInstance.GetComponent<MoneyPurse>();
            int recordedMoney = default;
            moneyPurseComponent.currentMoney = 420;
            moneyPurseComponent.MoneyChanged += amount => recordedMoney = amount;
            yield return null;

            Assert.AreNotEqual(default, recordedMoney);
            Assert.AreEqual(420, recordedMoney);
        }

        [UnityTest]
        public IEnumerator MoneyPurse_CanTellYou_IfYouCanAfford()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            var moneyPurseComponent = prefabInstance.GetComponent<MoneyPurse>();
            var dummyCostable = new GameObject();
            var costComponent = dummyCostable.AddComponent<Cost>();
            yield return null;

            var recordedCanAfford = false;
            moneyPurseComponent.PriceChecked += (_, canAfford) => recordedCanAfford = canAfford;
            costComponent.price = moneyPurseComponent.currentMoney - 1;
            Assert.IsTrue(moneyPurseComponent.PriceCheck(costComponent));
            Assert.IsTrue(recordedCanAfford);
            costComponent.price = moneyPurseComponent.currentMoney + 1;
            Assert.IsFalse(moneyPurseComponent.PriceCheck(costComponent));
            Assert.IsFalse(recordedCanAfford);
        }

        [UnityTest]
        public IEnumerator MoneyPurse_AllowsPlayer_ToPurchase()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            var moneyPurseComponent = prefabInstance.GetComponent<MoneyPurse>();
            var purchased = false;
            moneyPurseComponent.MoneyChanged += _ => purchased = true;
            moneyPurseComponent.currentMoney = 100;
            var dummyCostable = new GameObject();
            var costComponent = dummyCostable.AddComponent<Cost>();
            costComponent.price = 101;
            yield return null;

            moneyPurseComponent.Purchase(costComponent);

            Assert.IsTrue(purchased);
        }

        [UnityTest]
        public IEnumerator MoneyPurse_AllowsPlayers_ToEarnRewardMoney()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            var moneyPurseComponent = prefabInstance.GetComponent<MoneyPurse>();
            var addedReward = false;
            moneyPurseComponent.MoneyChanged += _ => addedReward = true;
            moneyPurseComponent.currentMoney = 0;
            var dummyReward = new GameObject();
            var rewardComponent = dummyReward.AddComponent<Reward>();
            rewardComponent.reward = 10;
            yield return null;

            Assert.AreEqual(0, moneyPurseComponent.currentMoney);
            moneyPurseComponent.AddReward(rewardComponent);
            Assert.IsTrue(addedReward);
            Assert.AreEqual(10, moneyPurseComponent.currentMoney);
        }

        [UnityTest]
        public IEnumerator MoneyPurse_CanBeRendered_InGUI()
        {
            var prefabInstance = _prefabSpawner.Spawn();
            CleanupAtEnd(prefabInstance);
            TestCameraLookAt(prefabInstance.transform);
            var moneyDisplay = _displayGuiSpawner.Spawn();
            CleanupAtEnd(moneyDisplay);
            var moneyPurseComponent = prefabInstance.GetComponent<MoneyPurse>();
            var moneyPurseDisplayComponent = moneyDisplay.GetComponent<MoneyPurseDisplay>();
            moneyPurseDisplayComponent.moneyPurse = moneyPurseComponent;
            moneyPurseComponent.currentMoney = 0;
            yield return null;

            Assert.AreEqual("Gold 0", moneyPurseDisplayComponent.Text());
            var dummyReward = new GameObject();
            CleanupAtEnd(dummyReward);
            var rewardComponent = dummyReward.AddComponent<Reward>();
            rewardComponent.reward = 10;
            moneyPurseComponent.AddReward(rewardComponent);
            Assert.AreEqual("Gold 10", moneyPurseDisplayComponent.Text());

            var dummyCost = new GameObject();
            CleanupAtEnd(dummyCost);
            var costComponent = dummyReward.AddComponent<Cost>();
            costComponent.price = 10;
            moneyPurseComponent.Purchase(costComponent);

            Assert.AreEqual("Gold 0", moneyPurseDisplayComponent.Text());
        }
    }
}
