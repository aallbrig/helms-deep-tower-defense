using System;
using Model.Controllers;
using UnityEngine;
using XR;

namespace MonoBehaviours.Controllers
{
    public class VRInputController : MonoBehaviour, IVRInputController
    {
        public bool debugEnabled;
        private XRIInputActions _inputActions;
        private void Awake() => _inputActions = new XRIInputActions();
        private void OnEnable() => _inputActions.Enable();
        private void OnDisable() => _inputActions.Disable();
        private void Start()
        {
            _inputActions.XRIHead.Position.started += ctx => DebugLog(ctx.ToString());
            _inputActions.XRIHead.Position.canceled += ctx => DebugLog(ctx.ToString());
            _inputActions.XRIHead.Rotation.started += ctx => DebugLog(ctx.ToString());
            _inputActions.XRIHead.Rotation.canceled += ctx => DebugLog(ctx.ToString());
        }
        private void DebugLog(string msg)
        {
            if (debugEnabled) Debug.Log($"{name} | <XRInputController> | {msg}");
        }
    }
}