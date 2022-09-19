using UnityEngine;

namespace MonoBehaviours.UI
{
    public class WorldSpaceCanvas : MonoBehaviour
    {
        private void Update() => transform.rotation = Camera.main.transform.rotation;
    }
}