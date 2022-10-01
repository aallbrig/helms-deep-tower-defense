using System;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class MainMenu : MonoBehaviour
    {
        public event Action MainMenuActivated;
        public GameObject canvas;
        private void Start()
        {
            canvas.SetActive(false);
        }
    }
}
