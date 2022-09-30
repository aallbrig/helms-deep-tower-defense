using System;
using UnityEngine;

namespace MonoBehaviours.Systems
{
    public class GameReferee : MonoBehaviour
    {
        public event Action GameIsOver;
        public event Action AllCastlesDestroyed;
        public event Action<GameObject> CastleRegistered;
        public int castlesAlive = 0;

        private void Start()
        {
            AllCastlesDestroyed += () => GameIsOver?.Invoke();
            // for each castle in the scene, listen for each "killed" event
            foreach (var castleComponent in FindObjectsOfType<Castle>())
            {
                castlesAlive++;
                castleComponent.Killed += OnCastleKilled;
                CastleRegistered?.Invoke(castleComponent.gameObject);
            }
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
