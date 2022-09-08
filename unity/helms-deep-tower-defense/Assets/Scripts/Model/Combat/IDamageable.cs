using System;

namespace Model.Combat
{
    public interface IDamageable<in T>
    {
        public event Action Damaged;
        public void Damage(T damage);
    }
}