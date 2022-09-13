using System;
using System.Collections.Generic;
using Model.Combat;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonoBehaviours.Combat
{
    public class AttackPoints : MonoBehaviour, IAssignAttackPoints
    {
        public event Action<Transform> AttackPointAssigned;
        public List<Transform> attackPoints = new List<Transform>();

        private void Start()
        {
            if (attackPoints.Count == 0) attackPoints.Add(transform);
        }
        public Transform AssignAttackPoint()
        {
            var attackPointAssignment = attackPoints[Random.Range(0, attackPoints.Count)];
            AttackPointAssigned?.Invoke(attackPointAssignment);
            return attackPointAssignment;
        }
    }
}