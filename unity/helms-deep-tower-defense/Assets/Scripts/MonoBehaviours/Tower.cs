using System;
using Model.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours
{
    public class Tower : MonoBehaviour, IDamageable, IKillable, ITowerConfig, IAttack, IHaveHealth
    {
        public TowerConfig towerConf;
        private float _currentHealth;

        private void Awake()
        {
            towerConf ??= ScriptableObject.CreateInstance<TowerConfig>();
            _currentHealth = MaxHealth;
        }
        private void Start()
        {
            Killed += () => gameObject.SetActive(false);
        }

        public event Action<float> Damaged;

        public void Damage(float damage)
        {
            if (_currentHealth == 0) return;

            _currentHealth -= damage;
            Damaged?.Invoke(damage);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Kill();
            }
        }

        public event Action Killed;

        public void Kill()
        {
            Killed?.Invoke();
        }

        public float MaxHealth => towerConf.MaxHealth;
        public float Range => towerConf.Range;

        public event Action<GameObject> AttackedTarget;

        public void Attack(GameObject target)
        {
            if (target.TryGetComponent<IDamageable>(out var damageable))
            {
                AttackedTarget?.Invoke(target);
            }
        }
        public float CurrentHealthNormalized() => _currentHealth / MaxHealth;
    }
}