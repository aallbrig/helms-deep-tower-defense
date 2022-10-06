using MonoBehaviours.Containers;
using UnityEditor;
using UnityEngine;

namespace Editor.ForMonoBehaviours.Containers
{
    [CustomEditor(typeof(Enemies))]
    public class EnemiesCustomEditor : UnityEditor.Editor
    {
        private readonly Vector3 _aboveCharacterOffset = new Vector3(0, 2f, 0);
        private Camera _camera;
        private Enemies _enemies;
        private void OnEnable()
        {
            _camera = Camera.main;
            _enemies = (Enemies)target;
        }
        public void OnSceneGUI() => _enemies.enemies.ForEach(enemy =>
        {
            var transformPosition = enemy.transform.position;
            var abovePosition = transformPosition + _aboveCharacterOffset;
            var screenPosition = _camera.WorldToScreenPoint(abovePosition);
            // TODO: if max draw distance, don't draw
            var distanceFromCamera = screenPosition.z / 20;
            var content = new GUIContent { text = $"{enemy.transform.name}" };
            var style = new GUIStyle { fontSize = (int)Mathf.Lerp(20, 6, distanceFromCamera) };
            Handles.Label(abovePosition, content, style);
        });
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
