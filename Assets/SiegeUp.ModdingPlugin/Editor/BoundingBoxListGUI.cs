using UnityEditor;
using UnityEngine;

namespace SiegeUp.ModdingPlugin
{
    [CustomEditor(typeof(BoundingBoxList))]
    public class BoundingBoxListGUI : Editor
    {
        BoundingBoxList targetObject;

        private void OnEnable() => targetObject = (BoundingBoxList)target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate/update common boundingBox"))
            {
                targetObject.UpdateMainBound();
                EditorUtility.SetDirty(targetObject);
            }
        }
    }
}