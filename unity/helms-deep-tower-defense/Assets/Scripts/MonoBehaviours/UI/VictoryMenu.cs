using System;
using MonoBehaviours.Systems;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class VictoryMenu : MonoBehaviour
    {
        public event Action VictoryMenuActivated;
        public GameObject menuCanvas;
        private bool _gameIsOver = false;
        private bool _allWavesCompleted = false;
        private bool _allEnemiesKilled = false;

        private void Start()
        {
            menuCanvas.SetActive(false);
            var referee = FindObjectOfType<GameReferee>();
            referee.GameIsOver += () =>
            {
                _gameIsOver = true;
                if (ShouldShowMenu()) ShowMenu();
            };
            referee.AllWavesSpawnsCompleted += () =>
            {
                _allWavesCompleted = true;
                if (ShouldShowMenu()) ShowMenu();
            };
            referee.AllEnemiesKilled += () =>
            {
                _allEnemiesKilled = true;
                if (ShouldShowMenu()) ShowMenu();
            };
        }

        private void ShowMenu()
        {
            if (menuCanvas == null) return;
            menuCanvas.SetActive(true);
            VictoryMenuActivated?.Invoke();
        }

        private bool ShouldShowMenu() => _gameIsOver && _allWavesCompleted && _allEnemiesKilled;
    }
}
