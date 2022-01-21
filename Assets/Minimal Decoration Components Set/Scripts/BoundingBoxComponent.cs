using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class BoundingBoxComponent : MonoBehaviour
{
    public bool passable = false;
    private const int SnapStep = 1;

    public Vector3 size()
    {
        var tmpRawSize = rawSize();
        var actualSize = new Vector3(
            snapValue(tmpRawSize.x, SnapStep),
            0,
            snapValue(tmpRawSize.z, SnapStep));
        return actualSize;
    }

    public Rect getRect(Quaternion rotation)
    {
        var rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, rotation.eulerAngles.y, 0));

        var box = GetComponent<BoxCollider>();
        var size = rawSize();
        var topLeft = box.center - size / 2;
        var bottomRight = box.center + size / 2;

        var topLeftRot = rotMatrix * topLeft;
        var bottomRightRot = rotMatrix * bottomRight;

        var newMin = new Vector2(Mathf.Min(topLeftRot.x, bottomRightRot.x), Mathf.Min(topLeftRot.z, bottomRightRot.z));
        var newMax = new Vector2(Mathf.Max(topLeftRot.x, bottomRightRot.x), Mathf.Max(topLeftRot.z, bottomRightRot.z));

        var rect = new Rect(
            newMin.x,
            newMin.y,
            newMax.x - newMin.x,
            newMax.y - newMin.y);

        return rect;
    }

    public Vector3 rawSize()
    {
        var result = GetComponent<BoxCollider>().size;
        result.Scale(transform.lossyScale);
        return result;
    }

    public Vector3 position()
    {
        var boxCollider = GetComponent<BoxCollider>();
        var result = boxCollider.center + transform.localPosition;
        result.y = 0;
        return result;
    }

    float snapValue(float value, float snapStep)
    {
        return Mathf.Ceil(value / snapStep) * snapStep;
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        if (Application.isPlaying)
            return;
        var boxCollider = GetComponent<BoxCollider>();
        boxCollider.size -= new Vector3(0, boxCollider.size.y, 0);
        boxCollider.center -= new Vector3(0, boxCollider.center.y, 0);
        transform.localPosition -= new Vector3(0, transform.localPosition.y, 0);
    }

    public void drawWire(Vector3 rootPosition, Color color)
    {
        var actualSize = size();

        var rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));


        var boxYOffset = new Vector3(0, transform.localPosition.y + GetComponent<BoxCollider>().center.y, 0);
        var rotatedSize = rotMatrix * actualSize;
        Vector3 rotatedOffset = new Vector3(rotatedSize.x / 2, 0, rotatedSize.z / 2);

        Vector3 rotatedPosition = rotMatrix * position();

        Vector3 rotatedSnapStep = rotMatrix * new Vector3(SnapStep, 0, SnapStep);

        Vector3 start = rootPosition + rotatedPosition - rotatedOffset + rotatedSnapStep * 0.5f - boxYOffset;
        int xNum = Mathf.CeilToInt(Mathf.Abs(rotatedSize.x) / SnapStep - 0.1f);
        int zNum = Mathf.CeilToInt(Mathf.Abs(rotatedSize.z) / SnapStep - 0.1f);

        for (int i = 0; i < xNum; i++)
        {
            for (int j = 0; j < zNum; j++)
            {
                var offset = new Vector3(rotatedSnapStep.x * i, 0, rotatedSnapStep.z * j);
                var oldColor = Gizmos.color;
                Gizmos.color = color;
                Gizmos.DrawWireCube(start + offset, new Vector3(rotatedSnapStep.x, 0, rotatedSnapStep.z));
                Gizmos.color = oldColor;
            }
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            if (transform.parent)
                drawWire(transform.parent.position, Color.green);
        }
        finally { }
    }
#endif
}
