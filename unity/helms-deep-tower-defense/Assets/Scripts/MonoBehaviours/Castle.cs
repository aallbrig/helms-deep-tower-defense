using System;
using Model.Combat;
using MonoBehaviours.Combat;
using UnityEngine;

namespace MonoBehaviours
{
    public class Castle : MonoBehaviour, IDamageable, IKillable, IHaveHealth
    {
        // Gotta work around limitations of [SerializeReference]
        public HealthSlider slider;
        public float maxHealth = 100f;
        private float _currentHealth;

        private void Awake()
        {
            if (slider)
            {
                slider.Damageable = this;
                slider.TrackHealthOf = this;
            }
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

        public void Kill()
        {
            Killed?.Invoke();
        }
        public float CurrentHealthNormalized() => _currentHealth / maxHealth;
    }
}