using System;
using UnityEngine;

namespace Model.Combat
{
    public interface IAttack
    {
        public event Action<GameObject> AttackedTarget;
        public void Attack(GameObject target);
    }
}