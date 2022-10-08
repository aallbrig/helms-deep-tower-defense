using System;
using System.Collections.Generic;
using System.Linq;
using CleverCrow.Fluid.FSMs;
using Generated;
using Model.Factories;
using MonoBehaviours.Commerce;
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
        public LayerMask obstaclesLayer;
        public Transform parentTransform;
        public float placementRayLength = 200f;
        [SerializeField] private GameObject activeTower;
        public List<TowerBuyButton> towerBuyButtons = new List<TowerBuyButton>();
        private readonly List<GameObject> _activeTowerPreviewIndicators = new List<GameObject>();
        private readonly Vector3 _activeTowerPreviewLocation = new Vector3(0, 100, 0);
        [SerializeReference] private IFsm _builderStateMachine;
        private Ray _indicatorPlacementRay;
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
                            ResetActiveTowerPreviews();
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
        private void OnDrawGizmos()
        {
            if (_indicatorPlacementRay.direction != default)
                Debug.DrawRay(_indicatorPlacementRay.origin, _indicatorPlacementRay.direction * placementRayLength,
                    Color.red);
        }

        public event Action<GameObject> Spawned;

        public GameObject Spawn()
        {
            if (activeTower == null || TowerNotPlaceable()) return null;
            var moneyPurse = FindObjectOfType<MoneyPurse>();
            var towerCost = activeTower.GetComponent<ICostMoney>();
            if (moneyPurse != null)
                if (moneyPurse.PriceCheck(towerCost))
                    moneyPurse.Purchase(towerCost);
                else
                    return null;

            var newTower = CreateNewTower();

            Spawned?.Invoke(newTower);
            ResetActiveTower();
            return newTower;
        }

        private GameObject CreateNewTower()
        {
            GameObject newTower;
            if (parentTransform != null)
            {
                newTower = Instantiate(activeTower, parentTransform);
                newTower.transform.position = indicator.transform.position;
                newTower.transform.rotation = indicator.transform.rotation;
            }
            else
            {
                newTower = Instantiate(activeTower, indicator.transform.position, indicator.transform.rotation);
            }
            return newTower;
        }
        private bool TowerNotPlaceable()
        {
            Physics.Raycast(
                indicator.position + new Vector3(0, -1, 0),
                Vector3.up,
                out var hit,
                10f,
                obstaclesLayer
            );
            return hit.collider != null;
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
            _indicatorPlacementRay = Camera.main.ScreenPointToRay(pointerPosition);
            if (Physics.Raycast(_indicatorPlacementRay, out var hit, placementRayLength, validBuildLayer))
                indicator.position = hit.point;

            indicator.gameObject.SetActive(!TowerNotPlaceable());
        }

        public event Action ActiveTowerToBuildReset;

        private void ResetActiveTower()
        {
            activeTower = null;
            ActiveTowerToBuildReset?.Invoke();
        }

        private void OnTowerBuyButtonClicked(GameObject tower)
        {
            activeTower = tower;
            SetActiveTowerIndicator();
        }
        private void SetActiveTowerIndicator()
        {
            var activeTowerIndicator = GetActiveTowerPreview();
            indicator = activeTowerIndicator.transform;
            PreviewIndicatorReplaced?.Invoke(activeTowerIndicator);
        }
        private void ResetActiveTowerPreviews()
        {
            foreach (var towerInstance in _activeTowerPreviewIndicators.Where(_ =>
                         _.transform.position != _activeTowerPreviewLocation))
                towerInstance.transform.position = _activeTowerPreviewLocation;
        }
        private GameObject GetActiveTowerPreview()
        {
            var towerPreview = _activeTowerPreviewIndicators.Find(preview => preview.name == activeTower.name);
            if (towerPreview == null)
            {
                towerPreview = Instantiate(activeTower, transform);
                towerPreview.name = $"{activeTower.name}";
                if (towerPreview.TryGetComponent<Tower>(out var tower)) tower.enabled = false;
                if (towerPreview.TryGetComponent<Collider>(out var towerCollider)) towerCollider.enabled = false;
                foreach (var childCollider in towerPreview.GetComponentsInChildren<Collider>()) childCollider.enabled = false;
                _activeTowerPreviewIndicators.Add(towerPreview);
            }
            return towerPreview;
        }

        public event Action<GameObject> PreviewIndicatorReplaced;
    }
}
