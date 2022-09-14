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
        private List<GameObject> _trackedTargets = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!_trackedTargets.Contains(other.gameObject))
                {
                    _trackedTargets.Add(other.gameObject);
                    TriggerEntered?.Invoke(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (_trackedTargets.Contains(other.gameObject))
                {
                    _trackedTargets.Remove(other.gameObject);
                    TriggerExited?.Invoke(other.gameObject);
                }
            }
        }
    }
}