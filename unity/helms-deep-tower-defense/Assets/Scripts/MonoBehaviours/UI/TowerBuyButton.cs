using System;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviours.UI
{
    [RequireComponent(typeof(Button))]
    public class TowerBuyButton : MonoBehaviour
    {
        public event Action<GameObject> TowerBuyButtonClicked;
        public GameObject prefab;
        public Button button;
        private void Start()
        {
            if (prefab == null) Debug.LogException(new Exception($"{name} is required to have a 'tower' prefab"));
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }
        private void OnButtonClick()
        {
            TowerBuyButtonClicked?.Invoke(prefab);
        }
    }
}