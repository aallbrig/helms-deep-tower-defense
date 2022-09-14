using System;
using Model.Combat;
using MonoBehaviours.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours
{
    public class Tower : MonoBehaviour, IDamageable, IKillable, ITowerConfig
    {
        public TowerConfig towerConf;
        private float _currentHealth;

        private void Start()
        {
            towerConf ??= ScriptableObject.CreateInstance<TowerConfig>();
            _currentHealth = towerConf.MaxHealth;
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
    }
}