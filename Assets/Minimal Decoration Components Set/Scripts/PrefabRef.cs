using UnityEngine;
using System;

[ExecuteInEditMode]
public class PrefabRef : MonoBehaviour
{
    public string prefabId;
    public bool ignore;

#if UNITY_EDITOR
    void Awake() => PrefabManager.updatePrefabManager();
#endif
}
