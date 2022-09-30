using System;
using Model.Combat;
using UnityEngine;

namespace MonoBehaviours
{
    public class Castle : MonoBehaviour, IDamageable, IKillable, IHaveHealth
    {
        // Gotta work around limitations of [SerializeReference], meaning I can't just use interfaces
        public float maxHealth = 100f;
        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        private void Start()
        {
            Killed += () => gameObject.SetActive(false);
        }

        public event Action<float> Damaged;
        public void Damage(float damage)
        {
            if (_currentHealth <= 0) return;
            _currentHealth -= damage;
            Damaged?.Invoke(damage);
            if (_currentHealth <= 0) Killed?.Invoke();
        }

        public event Action Killed;

        public bool IsDead => _currentHealth <= 0;

        public void Kill() => Damage(maxHealth);
        public float CurrentHealthNormalized() => _currentHealth / maxHealth;
    }
}
