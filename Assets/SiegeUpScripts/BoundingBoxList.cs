using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class BoundingBoxList : MonoBehaviour
{
    public List<BoundingBoxComponent> boundingBoxes;

#if UNITY_EDITOR
    [SerializeField] private bool AutoUpdateBounds = true;
#endif

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

    private void LateUpdate()
    {
        if (AutoUpdateBounds && boundingBoxes.Count > 0)
            UpdateMainBound();
    }

    [ContextMenu("Find all bounds")]
    public void findAllBounds()
    {
        boundingBoxes = new List<BoundingBoxComponent>(GetComponentsInChildren<BoundingBoxComponent>());
    }

    public static Bounds GetCommonBounds(GameObject parent)
    {
        var renderers = parent.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0)
            return new Bounds();
        Bounds commonBounds = renderers[0].bounds;
        foreach (var renderer in renderers)
            commonBounds.Encapsulate(renderer.bounds);
        return commonBounds;
    }

    public void UpdateMainBound()
    {
        BoxCollider boxCollider;
        findAllBounds();
        if (boundingBoxes.Count == 0)
        {
            var boundingBox = new GameObject("BoundingBox");
            boundingBox.transform.parent = transform;
            boundingBoxes.Add(boundingBox.AddComponent<BoundingBoxComponent>());
        }
        boxCollider = boundingBoxes[0].GetComponent<BoxCollider>();
        var bounds = GetCommonBounds(gameObject);
        boxCollider.transform.localPosition = Vector3.zero;
        boxCollider.transform.localScale = Vector3.one;
        boxCollider.size.Set(bounds.size.x, 0, bounds.size.z);
        boxCollider.center.Set(bounds.center.x - boxCollider.transform.position.x, 0, bounds.center.z - boxCollider.transform.position.z);
    }
#endif
}
