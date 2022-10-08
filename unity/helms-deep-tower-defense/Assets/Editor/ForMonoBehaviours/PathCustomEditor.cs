#if UNITY_EDITOR
using System.Collections.Generic;
using MonoBehaviours;
using UnityEditor;
using UnityEngine;

namespace Editor.ForMonoBehaviours
{
    [CustomEditor(typeof(Path))]
    public class PathCustomInspector : UnityEditor.Editor
    {
        private readonly Vector3 _aboveGroundOffset = new Vector3(0, 0.1f, 0);
        private Camera _camera;
        private Path _path;
        private void OnEnable()
        {
            _path = (Path)target;
            _camera = Camera.main;
        }
        public void OnSceneGUI()
        {
            Handles.color = Color.red;
            var i = 0;
            var lineSegmentPairs = new List<Vector3>();
            _path.pathPoints
                .ForEach(pathPoint =>
                {
                    var transformPosition = pathPoint.position;
                    var aboveTransformPosition = transformPosition + _aboveGroundOffset;
                    var screenPosition = _camera.WorldToScreenPoint(aboveTransformPosition);
                    // TODO: if max draw distance, don't draw
                    var distanceFromCamera = screenPosition.z / 20;
                    var content = new GUIContent { text = $"point: {pathPoint.name}" };
                    var style = new GUIStyle
                    {
                        fontSize = (int)Mathf.Lerp(20, 6, distanceFromCamera)
                    };
                    Handles.Label(transformPosition + _aboveGroundOffset, content, style);
                    Handles.DrawWireDisc(aboveTransformPosition, Vector3.up, 0.5f);

                    var nextI = i + 1;

                    if (nextI < _path.pathPoints.Count)
                    {
                        lineSegmentPairs.Add(transformPosition);
                        lineSegmentPairs.Add(_path.pathPoints[nextI].position);
                    }
                    i++;
                });
            Handles.DrawLines(lineSegmentPairs.ToArray());
        }
        public override void OnInspectorGUI() => base.OnInspectorGUI();
    }
}
#endif
