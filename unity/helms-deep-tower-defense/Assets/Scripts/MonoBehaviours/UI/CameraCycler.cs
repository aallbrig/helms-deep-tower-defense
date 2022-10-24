using System;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace MonoBehaviours.UI
{
    public class CameraCycler : MonoBehaviour
    {
        public CinemachineVirtualCamera[] cameras = {};
        private int _currentIndex;
        private void Start()
        {
            InitializeCameras();
            SyncCameraState();
        }

        private void InitializeCameras() =>
            // sort the list of cinemachine virtual cameras by priority
            // optimization note: yes linq creates garbage but this method is called only once so honestly no biggie
            // ... and yea, if my scene gets too big and/or hierarchical, I'll want to consider changing FindObjectsOfType
            // API call
            cameras = (
                from v in FindObjectsOfType<CinemachineVirtualCamera>(true)
                orderby v.Priority descending
                select v
            ).ToArray();

        public event Action CycledForwards;

        public event Action CycledBackwards;

        public void Forward()
        {
            if (cameras.Length == 0) return;
            UpdateCurrentIndex(1);
            SyncCameraState();
            CycledForwards?.Invoke();
        }
        public void Backwards()
        {
            if (cameras.Length == 0) return;
            UpdateCurrentIndex(-1);
            SyncCameraState();
            CycledBackwards?.Invoke();
        }
        private void SyncCameraState()
        {
            for (var i = 0; i < cameras.Length; i++)
                cameras[i].gameObject.SetActive(i == _currentIndex);
        }
        private void UpdateCurrentIndex(int change)
        {
            _currentIndex += change;
            if (_currentIndex < 0) _currentIndex = cameras.Length - 1;
            if (_currentIndex >= cameras.Length) _currentIndex = 0;
        }
    }
}
