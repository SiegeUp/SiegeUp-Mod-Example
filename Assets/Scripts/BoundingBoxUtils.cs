using UnityEngine;

public class BoundingBoxUtils
{
    public static Bounds GetCommonBounds(GameObject parent)
	{
        var renderers = parent.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0)
            return new Bounds();
        Bounds commonBounds = renderers[0].bounds;
        foreach (var renderer in renderers)
        {
            commonBounds.Encapsulate(renderer.bounds.center + renderer.bounds.size / 2);
            commonBounds.Encapsulate(renderer.bounds.center - renderer.bounds.size / 2);
        }
        return commonBounds;
    }
}