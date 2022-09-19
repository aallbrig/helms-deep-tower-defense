using System;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Combat
{
    public class Team : MonoBehaviour, ITeamConfig
    {
        public TeamConfiguration config;
        private void Awake()
        {
            if (config == null) Debug.LogException(new Exception($"{name} Team component requires a team configuration"));
        }

        public LayerMask allies => config.allies;

        public LayerMask enemies => config.enemies;
    }
}