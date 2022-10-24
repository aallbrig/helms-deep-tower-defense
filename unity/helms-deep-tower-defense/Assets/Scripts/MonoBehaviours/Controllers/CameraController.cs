using Generated;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace MonoBehaviours.Controllers
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public float touchSpeed = 10f;
        public float slowestZoomDelta = 5f;
        public float fastestZoomDelta = 30f;
        private Camera _camera;
        private PlayerInputActions _input;
        private float _lastMultiTouchDistance;
        private Transform _transform;
        private void Awake()
        {
            _transform = transform;
            _camera = GetComponent<Camera>();
            _input = new PlayerInputActions();
            EnhancedTouchSupport.Enable();
        }
        private void Update()
        {
            if (Touch.activeFingers.Count == 2)
                HandleTwoFingerTouch(Touch.activeTouches[0], Touch.activeTouches[1]);
        }
        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();
        private void HandleTwoFingerTouch(Touch firstTouch, Touch secondTouch)
        {
            if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
                _lastMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition,
                    secondTouch.screenPosition);

            if (firstTouch.phase != TouchPhase.Moved ||
                secondTouch.phase != TouchPhase.Moved)
                return;

            var newMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition,
                secondTouch.screenPosition);

            var multiTouchDistanceDelta = Mathf.Abs(newMultiTouchDistance - _lastMultiTouchDistance);
            var zoomIn = newMultiTouchDistance < _lastMultiTouchDistance;
            var zoomSpeed = Mathf.Lerp(1f, 10f, multiTouchDistanceDelta);
            // if distance is greater, the player is trying to zoom out
            // if distance is lesser, the player is trying to zoom in
            // TODO: player tries to rotate camera

            _lastMultiTouchDistance = newMultiTouchDistance;
        }
    }
}
