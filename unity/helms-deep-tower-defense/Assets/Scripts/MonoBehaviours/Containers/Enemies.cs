using System.Collections.Generic;
using System.Linq;
using Model.Combat;
using Model.Factories;
using MonoBehaviours.Combat;
using ScriptableObjects;
using UnityEngine;

namespace MonoBehaviours.Containers
{
    public class Enemies : MonoBehaviour
    {
        public TeamConfiguration badGuyTeam;
        public List<GameObject> enemies;
        private void Awake()
        {
            foreach (var spawner in FindObjectsOfType<MonoBehaviour>(true).OfType<ISpawner>())
                spawner.Spawned += enemy =>
                {
                    if (enemy.TryGetComponent<Team>(out var team))
                        if (team.config == badGuyTeam)
                        {
                            enemies.Add(enemy);
                            if (enemy.TryGetComponent<IKillable>(out var killable))
                                killable.Killed += () => enemies.Remove(enemy);
                        }
                };

        }
    }
}
