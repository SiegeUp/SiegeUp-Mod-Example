using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxList : MonoBehaviour
{
    public List<BoundingBoxComponent> boundingBoxes;

    public BoundingBoxComponent getMainBound()
    {
        return boundingBoxes.Count > 0 ? boundingBoxes[0] : null;
    }

#if UNITY_EDITOR
    [ContextMenu("Find all bounds")]
    public void findAllBounds()
    {
        boundingBoxes = new List<BoundingBoxComponent>(GetComponentsInChildren<BoundingBoxComponent>());
        UnityEditor.EditorUtility.SetDirty(gameObject);
    }
#endif
}
