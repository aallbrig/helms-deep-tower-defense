using System;
using System.Collections.Generic;
using CleverCrow.Fluid.FSMs;
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

        public enum BuilderStates
        {
            Awaiting,
            PreparingBuild
        }

        public Transform indicator;
        public LayerMask validBuildLayer;
        [SerializeField] private GameObject activeTower;
        public List<TowerBuyButton> towerBuyButtons = new List<TowerBuyButton>();
        [SerializeReference] private IFsm _builderStateMachine;
        private PlayerInputActions _input;
        private void Awake()
        {
            _input = new PlayerInputActions();
            _builderStateMachine = new FsmBuilder()
                .Owner(gameObject)
                .Default(BuilderStates.Awaiting)
                .State(BuilderStates.Awaiting, stateBuilder =>
                {
                    stateBuilder
                        .SetTransition($"{BuilderStates.PreparingBuild}", BuilderStates.PreparingBuild)
                        .Update(action =>
                        {
                            if (PlayerBuildIntent()) action.Transition($"{BuilderStates.PreparingBuild}");
                        });
                })
                .State(BuilderStates.PreparingBuild, stateBuilder =>
                {
                    stateBuilder
                        .SetTransition($"{BuilderStates.Awaiting}", BuilderStates.Awaiting)
                        .Enter(action =>
                        {
                            indicator.gameObject.SetActive(true);
                            _input.Gameplay.PointerPosition.performed += OnPointerPositionMove;
                            _input.Gameplay.PointerClick.started += OnPointerClicked;
                        })
                        .Exit(action =>
                        {
                            indicator.gameObject.SetActive(false);
                            _input.Gameplay.PointerPosition.performed -= OnPointerPositionMove;
                            _input.Gameplay.PointerClick.started -= OnPointerClicked;
                        })
                        .Update(action =>
                        {
                            if (!PlayerBuildIntent()) action.Transition($"{BuilderStates.Awaiting}");
                        });
                })
                .Build();
        }

        private void Start()
        {
            towerBuyButtons.AddRange(FindObjectsOfType<TowerBuyButton>());
            towerBuyButtons.ForEach(towerBuyButton => towerBuyButton.TowerBuyButtonClicked += OnTowerBuyButtonClicked);
        }
        private void Update() => _builderStateMachine.Tick();
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
            ResetActiveTower();
            return newTower;
        }
        private void OnPointerClicked(InputAction.CallbackContext ctx) =>
            // Spawn a new tower at the current indicator transform position
            Spawn();
        private bool PlayerBuildIntent() =>
            // when the active tower is set when the player clicks on the "build tower" UI element
            activeTower != null;
        private void OnPointerPositionMove(InputAction.CallbackContext ctx)
        {
            var pointerPosition = ctx.ReadValue<Vector2>();
            Debug.Log($"pointer position: {pointerPosition}");
            var ray = Camera.main.ScreenPointToRay(pointerPosition);
            Debug.DrawRay(ray.origin, ray.direction * 200f, Color.red);
            if (Physics.Raycast(ray, out var hit, 200f, validBuildLayer))
                indicator.position = hit.point;
        }

        public event Action ActiveTowerToBuildReset;

        private void ResetActiveTower()
        {
            activeTower = null;
            ActiveTowerToBuildReset?.Invoke();
        }

        private void OnTowerBuyButtonClicked(GameObject tower) => activeTower = tower;
    }
}
