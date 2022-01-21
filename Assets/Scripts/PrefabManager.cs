using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrefabManager : ScriptableObject
{
#if UNITY_EDITOR
    public static void updatePrefabManager()
    {
        Debug.Log("Update prefab manager");
        var prefabRefs = Resources.FindObjectsOfTypeAll<PrefabRef>();
        foreach (var prefabRef in prefabRefs)
        {
            if (AssetDatabase.Contains(prefabRef.gameObject) && !AssetDatabase.IsSubAsset(prefabRef.gameObject) && prefabRef.transform.parent == null && !prefabRef.ignore)
            {
                var newId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabRef.gameObject));
                if (prefabRef.prefabId != newId)
                {
                    prefabRef.prefabId = newId;
                }
            }
        }
    }
#endif
}
