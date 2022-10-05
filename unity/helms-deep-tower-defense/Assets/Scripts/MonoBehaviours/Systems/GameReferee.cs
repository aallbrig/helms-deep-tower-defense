using System;
using Model.Combat;
using MonoBehaviours.Factories;
using UnityEngine;

namespace MonoBehaviours.Systems
{
    public class GameReferee : MonoBehaviour
    {
        public int castlesAlive;
        public int enemies;
        public int waves;

        private void Start()
        {
            AllCastlesDestroyed += () => GameIsOver?.Invoke();
            // for each castle in the scene, listen for each "killed" event
            foreach (var castleComponent in FindObjectsOfType<Castle>(true))
            {
                castlesAlive++;
                castleComponent.Killed += OnCastleKilled;
                CastleRegistered?.Invoke(castleComponent.gameObject);
            }
            foreach (var spawner in FindObjectsOfType<SpawnWave>())
            {
                spawner.Spawned += spawnedGameObject =>
                {
                    if (spawnedGameObject.TryGetComponent<IKillable>(out var killable))
                    {
                        enemies++;
                        killable.Killed += OnKillableKilled;
                    }
                };
                if (spawner.gameObject.TryGetComponent<SpawnWave>(out var spawnWave))
                {
                    waves++;
                    spawnWave.WaveCompleted += OnWaveCompleted;
                }
            }
        }

        public event Action GameHasStarted;

        public event Action GameIsOver;

        public event Action AllCastlesDestroyed;

        public event Action AllWavesSpawnsCompleted;

        public event Action AllEnemiesKilled;

        public event Action<GameObject> CastleRegistered;

        private void OnWaveCompleted()
        {
            waves--;
            if (waves == 0) AllWavesSpawnsCompleted?.Invoke();
            if (GoodGuysWinCondition())
                GameIsOver?.Invoke();
        }
        private bool GoodGuysWinCondition() => waves <= 0 && enemies <= 0;

        private void OnKillableKilled()
        {
            enemies--;
            if (enemies == 0) AllEnemiesKilled?.Invoke();
            if (GoodGuysWinCondition())
                GameIsOver?.Invoke();
        }

        private void OnCastleKilled()
        {
            castlesAlive--;
            // when all castles are destroyed, referee shouts "all castles destroyed"
            if (castlesAlive == 0)
                AllCastlesDestroyed?.Invoke();
        }
    }
}
