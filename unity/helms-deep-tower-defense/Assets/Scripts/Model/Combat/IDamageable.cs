using System;

namespace Model.Combat
{
    public interface IDamageable
    {
        public event Action<float> Damaged;
        public void Damage(float damage);
    }
}