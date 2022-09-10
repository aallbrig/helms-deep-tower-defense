using System;

namespace Model.Combat
{
    public interface IDamageable<T>
    {
        public event Action<T> Damaged;
        public void Damage(T damage);
    }
}