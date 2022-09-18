using UnityEngine;

namespace MonoBehaviours.UI
{
    public class WorldSpaceCanvas : MonoBehaviour
    {
        public Camera cameraPerspective;

        private void LateUpdate()
        {
            if (cameraPerspective == null) return;
            transform.LookAt(
                transform.position + cameraPerspective.transform.rotation * Vector3.forward,
                cameraPerspective.transform.rotation * Vector3.up
            );
        }
        private void OnEnable()
        {
            cameraPerspective ??= Camera.main;
            cameraPerspective ??= Camera.current;
        }
    }
}