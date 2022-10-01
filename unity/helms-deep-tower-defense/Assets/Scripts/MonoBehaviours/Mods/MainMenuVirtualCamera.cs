using System;
using Cinemachine;
using MonoBehaviours.UI;
using UnityEngine;

namespace MonoBehaviours.Mods
{
    public class MainMenuVirtualCamera : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        private void Awake()
        {
            if (ShouldDeactivateVCam()) DeactivateVCam();
        }
        private void Start()
        {
            var mainMenu = FindObjectOfType<MainMenu>();
            mainMenu.MainMenuActivated += () => virtualCamera.enabled = true;
        }
        private void DeactivateVCam()
        {
            virtualCamera.enabled = false;
        }
        private bool ShouldDeactivateVCam()
        {
            return true;
        }
    }
}
