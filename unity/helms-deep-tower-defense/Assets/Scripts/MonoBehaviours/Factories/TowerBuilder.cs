using System;
using System.Collections.Generic;
using Generated;
using Model.Factories;
using MonoBehaviours.Combat;
using MonoBehaviours.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MonoBehaviours.Factories
{
    public class TowerBuilder : MonoBehaviour, ISpawner
    {
        public Transform indicator;
        public LayerMask ground;
        [SerializeField] private GameObject activeTower;
        [SerializeField] private Tower activeTowerComponent;
        [SerializeField] private Team activeTeamComponent;
        public List<TowerBuyButton> towerBuyButtons = new List<TowerBuyButton>();
        private PlayerInputActions _input;
        private void Awake() => _input = new PlayerInputActions();

        private void Start()
        {
            towerBuyButtons.AddRange(FindObjectsOfType<TowerBuyButton>());
            towerBuyButtons.ForEach(towerBuyButton => towerBuyButton.TowerBuyButtonClicked += OnTowerBuyButtonClicked);
            _input.Gameplay.PointerPosition.performed += OnPointerPositionMove;
        }
        private void OnPointerPositionMove(InputAction.CallbackContext ctx)
        {
            var pointerPosition = ctx.ReadValue<Vector2>();
            Debug.Log($"pointer position: {pointerPosition}");
            var ray = Camera.main.ScreenPointToRay(pointerPosition);
            Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);
            if (Physics.Raycast(ray, out var hit, 200f, ground))
                indicator.position = hit.point;
        }
        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();

        public event Action<GameObject> Spawned;

        public GameObject Spawn()
        {
            if (activeTower == null) return null;
            var newTower = Instantiate(activeTower, indicator.transform.position, indicator.transform.rotation);
            if (newTower.TryGetComponent<Tower>(out var tower)) {}
            if (newTower.TryGetComponent<Team>(out var team)) {}
            Spawned?.Invoke(newTower);
            return newTower;
        }

        private void OnTowerBuyButtonClicked(GameObject tower)
        {
            activeTower = tower;
            activeTowerComponent = activeTower.GetComponent<Tower>();
            activeTeamComponent = activeTower.GetComponent<Team>();
            indicator.gameObject.SetActive(true);
        }
    }
}