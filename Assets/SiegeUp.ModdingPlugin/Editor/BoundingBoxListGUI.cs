using UnityEditor;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
    [CustomEditor(typeof(BoundingBoxList))]
    public class BoundingBoxListGUI : Editor
    {
        private BoundingBoxList _targetObject;

        private void OnEnable() => _targetObject = (BoundingBoxList)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate/update common boundingBox"))
            {
                _targetObject.UpdateMainBound();
                EditorUtility.SetDirty(_targetObject);
            }
        }
    }
}