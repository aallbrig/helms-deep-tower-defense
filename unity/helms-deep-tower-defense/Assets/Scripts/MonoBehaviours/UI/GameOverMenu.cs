using System;
using MonoBehaviours.Systems;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        public event Action GameOverMenuIsActivated;
        public GameObject gameOverMenuCanvas;
        private bool _allCastlesDestroyed = false;
        private bool _gameIsOver = false;
        private void Start()
        {
            gameOverMenuCanvas.SetActive(false);
            var referee = FindObjectOfType<GameReferee>();
            referee.AllCastlesDestroyed += () =>
            {
                _allCastlesDestroyed = true;
                if (ShouldActivateMenu()) ActivateMenu();
            };
            referee.GameIsOver += () =>
            {
                _gameIsOver = true;
                if (ShouldActivateMenu()) ActivateMenu();
            };
        }
        private void ActivateMenu()
        {
            if (gameOverMenuCanvas == null) return;

            gameOverMenuCanvas.SetActive(true);
            GameOverMenuIsActivated?.Invoke();
        }
        private bool ShouldActivateMenu() => _allCastlesDestroyed && _gameIsOver;
    }
}
