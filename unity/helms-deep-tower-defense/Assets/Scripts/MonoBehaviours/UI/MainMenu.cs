using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviours.UI
{
    public class MainMenu : MonoBehaviour
    {
        public event Action MainMenuActivated;

        public Scene newGame;
        public GameObject canvas;
        private void Start()
        {
            canvas.SetActive(true);
            MainMenuActivated?.Invoke();
        }
        public void NewGame() => SceneManager.LoadScene("Game", LoadSceneMode.Single);

        public void QuitGame() => Application.Quit();
    }
}
