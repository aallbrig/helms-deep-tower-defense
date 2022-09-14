using System;

namespace Model.Triggers
{
    public interface ITriggerCollider<T>
    {
        public event Action<T> TriggerEntered;
        public event Action<T> TriggerExited;
    }
}