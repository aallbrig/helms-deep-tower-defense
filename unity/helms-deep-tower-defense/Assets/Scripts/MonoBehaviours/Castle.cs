using System;
using System.Collections.Generic;
using Model.Combat;
using MonoBehaviours.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonoBehaviours
{
    public class Castle : MonoBehaviour, IDamageable, IKillable, IHaveHealth, IAssignAttackPoints
    {
        // Gotta work around limitations of [SerializeReference]
        public HealthSlider slider;
        public float maxHealth = 100f;
        public List<Transform> attackPoints = new List<Transform>();
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
            if (attackPoints.Count == 0) attackPoints.Add(transform);
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
        public Transform AssignAttackPoint()
        {
            return attackPoints[Random.Range(0, attackPoints.Count)];
        }
    }
}