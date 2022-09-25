using System.Collections;
using Model.Factories;
using MonoBehaviours.Commerce;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.PlayMode.Scenarios.ForMoneyPurse
{
    public class MoneyPurseFacts: ScenarioTest
    {
        private readonly PrefabSpawner _prefabSpawner = new PrefabSpawner("Prefabs/Systems/Money Purse");
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
    }
}
