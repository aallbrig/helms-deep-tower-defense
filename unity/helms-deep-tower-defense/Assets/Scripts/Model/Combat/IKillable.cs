using System;

namespace Model.Combat
{
    public interface IKillable
    {
        public event Action Killed;
        public bool IsDead { get; }
        public void Kill();
    }
}
