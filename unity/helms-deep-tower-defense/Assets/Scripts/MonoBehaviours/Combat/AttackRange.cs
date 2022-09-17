using System;
using System.Collections.Generic;
using Model.Combat;
using Model.Triggers;
using UnityEngine;

namespace MonoBehaviours.Combat
{
    public class AttackRange : MonoBehaviour, ITriggerCollider<GameObject>
    {
        public event Action<GameObject> TriggerEntered;
        public event Action<GameObject> TriggerExited;
        public LayerMask layerMaskFilter;
        public List<GameObject> trackedTargets = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (IsInLayerMaskFilter(other) && other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!trackedTargets.Contains(other.gameObject))
                {
                    trackedTargets.Add(other.gameObject);
                    TriggerEntered?.Invoke(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (IsInLayerMaskFilter(other) && other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (trackedTargets.Contains(other.gameObject))
                {
                    trackedTargets.Remove(other.gameObject);
                    TriggerExited?.Invoke(other.gameObject);
                }
            }
        }

        private bool IsInLayerMaskFilter(Collider other) => layerMaskFilter == (layerMaskFilter | (1 << other.gameObject.layer));
    }
}