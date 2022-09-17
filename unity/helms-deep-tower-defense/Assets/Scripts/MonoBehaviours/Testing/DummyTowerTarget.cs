using System;
using Model.Combat;
using UnityEngine;

namespace MonoBehaviours.Testing
{
    public class DummyTowerTarget : MonoBehaviour, IDamageable
    {

        public event Action<float> Damaged;

        public void Damage(float damage)
        {
            Damaged?.Invoke(damage);
        }
    }
}