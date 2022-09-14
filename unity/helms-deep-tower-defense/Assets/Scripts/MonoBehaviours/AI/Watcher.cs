using System;
using Model.Triggers;
using UnityEngine;

namespace MonoBehaviours.AI
{
    public class Watcher : MonoBehaviour
    {
        public GameObject triggerFrom;
        public Transform applyTo;
        private Transform _target;
        private ITriggerCollider<GameObject> _triggerCollider;
        private void Start()
        {
            applyTo ??= transform;
            triggerFrom ??= gameObject;
            if (triggerFrom.TryGetComponent<ITriggerCollider<GameObject>>(out var triggerCollider))
            {
                _triggerCollider = triggerCollider;
                _triggerCollider.TriggerEntered += _ =>
                {
                    if (_target == null) _target = _.transform;
                };
                _triggerCollider.TriggerExited += _ =>
                {
                    if (_target == _.transform) _target = null;
                };
            }
        }
        private void Update()
        {
            if (_target == null) return;

            applyTo.LookAt(_target);
        }
        private void CommonStartWatching(Transform target) => _target = target;
        private void CommonStopWatching(Transform target)
        {
            if (_target == target) _target = null;
        }
    }
}