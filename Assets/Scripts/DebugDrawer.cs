using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    [field: Header("General Settings")]
    [field: SerializeField] private float xBounds = 8.25f;
    [field: SerializeField] private float yBounds = 4.35f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 rightVerticalLineStartPosition = transform.position + transform.right * xBounds;
        Vector3 leftVerticalLineStartPosition = transform.position - transform.right * xBounds;
        Vector3 upperHorizontalLineStartPosition = transform.position + transform.up * yBounds;
        Vector3 lowerHorizontalLineStartPosition = transform.position - transform.up * yBounds;

        Gizmos.DrawLine(leftVerticalLineStartPosition + Vector3.down * yBounds, leftVerticalLineStartPosition + Vector3.up * yBounds);
        Gizmos.DrawLine(rightVerticalLineStartPosition + Vector3.down * yBounds, rightVerticalLineStartPosition + Vector3.up * yBounds);
        Gizmos.DrawLine(upperHorizontalLineStartPosition + Vector3.left * xBounds, upperHorizontalLineStartPosition + Vector3.right * xBounds);
        Gizmos.DrawLine(lowerHorizontalLineStartPosition + Vector3.left * xBounds, lowerHorizontalLineStartPosition + Vector3.right * xBounds);
    }
}