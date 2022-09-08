using System;
using Model.Combat;
using UnityEngine;

namespace MonoBehaviours
{
    public class Castle : MonoBehaviour, IDamageable<float>, IKillable
    {
        public Collider hitBox;
        public float maxHealth = 100f;
        private float _currentHealth;
        private void Start()
        {
            _currentHealth = maxHealth;
            hitBox ??= GetComponent<Collider>();
        }

        public event Action Damaged;
        public void Damage(float damage)
        {
            if (_currentHealth <= 0) return;
            _currentHealth -= damage;
            Damaged?.Invoke();
            if (_currentHealth <= 0) Killed?.Invoke();
        }

        public event Action Killed;

        public void Kill()
        {
            Killed?.Invoke();
        }
    }
}