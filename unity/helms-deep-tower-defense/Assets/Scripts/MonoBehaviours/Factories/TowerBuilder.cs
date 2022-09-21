using System;
using System.Collections.Generic;
using Model.Factories;
using MonoBehaviours.Combat;
using MonoBehaviours.UI;
using UnityEngine;

namespace MonoBehaviours.Factories
{
    public class TowerBuilder : MonoBehaviour, ISpawner
    {
        public Transform indicator;
        [SerializeField] private GameObject activeTower;
        [SerializeField] private Tower activeTowerComponent;
        [SerializeField] private Team activeTeamComponent;
        public List<TowerBuyButton> towerBuyButtons = new List<TowerBuyButton>();

        private void Start()
        {
            towerBuyButtons.AddRange(FindObjectsOfType<TowerBuyButton>());
            towerBuyButtons.ForEach(towerBuyButton =>
            {
                towerBuyButton.TowerBuyButtonClicked += OnTowerBuyButtonClicked;
            });
        }
        private void OnTowerBuyButtonClicked(GameObject tower)
        {
            activeTower = tower;
            activeTowerComponent = activeTower.GetComponent<Tower>();
            activeTeamComponent = activeTower.GetComponent<Team>();
            indicator.gameObject.SetActive(true);
        }

        public event Action<GameObject> Spawned;

        public GameObject Spawn()
        {
            var newTower = new GameObject();
            if (newTower.TryGetComponent<Team>(out var team)) {}
            Spawned?.Invoke(newTower);
            return newTower;
        }
    }
}