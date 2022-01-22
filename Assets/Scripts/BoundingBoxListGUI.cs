using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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
#endif