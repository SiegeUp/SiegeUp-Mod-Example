using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrefabRef : MonoBehaviour
{
    public string prefabId;
    public bool ignore;

#if UNITY_EDITOR
    public void Regenerate()
    {
        if (AssetDatabase.Contains(gameObject) && !AssetDatabase.IsSubAsset(gameObject) && transform.parent == null && !ignore)
        {
            var newId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(gameObject));
            if (prefabId != newId)
            {
                prefabId = newId;
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif
}
