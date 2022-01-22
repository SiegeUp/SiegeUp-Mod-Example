using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class BoundingBoxList : MonoBehaviour
{
    [SerializeField] private bool AutoUpdateBounds = true;
    public List<BoundingBoxComponent> boundingBoxes;

    public BoundingBoxComponent getMainBound()
    {
        return boundingBoxes.Count > 0 ? boundingBoxes[0] : null;
    }

#if UNITY_EDITOR
	private void OnEnable()
	{
        if (AutoUpdateBounds)
            UpdateMainBound();
    }

	[ContextMenu("Find all bounds")]
    public void findAllBounds()
    {
        boundingBoxes = new List<BoundingBoxComponent>(GetComponentsInChildren<BoundingBoxComponent>());
        EditorUtility.SetDirty(gameObject);
    }

    public void UpdateMainBound()
	{
        BoxCollider boxCollider;
        findAllBounds();
        if (boundingBoxes.Count == 0)
		{
            var boundingBox = new GameObject("BoundingBox");
            boundingBox.transform.parent = transform;
            boxCollider = boundingBox.AddComponent<BoxCollider>();
            boundingBoxes.Add(boundingBox.AddComponent<BoundingBoxComponent>());
        }
		else
		{
            boxCollider = boundingBoxes[0].GetComponent<BoxCollider>();
        }
        var bounds = BoundingBoxUtils.GetCommonBounds(gameObject);
        boxCollider.transform.localPosition = Vector3.zero;
        boxCollider.transform.localScale = Vector3.one;
        boxCollider.size = bounds.size;
        boxCollider.center = bounds.center - boxCollider.transform.position;
    }
#endif
}
