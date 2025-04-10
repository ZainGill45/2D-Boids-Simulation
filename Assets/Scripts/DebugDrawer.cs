using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    [field: Header("General Settings")]
    [field: SerializeField] private float lineLength = 0.5f;
    [field: SerializeField] private float xBounds = 6f;
    [field: SerializeField] private float yBounds = 4f;

    public static List<Boid> boids;

    private void Awake()
    {
        boids = new List<Boid>();
    }

    private void Update()
    {
        if (boids.Count > 0)
            foreach (Boid boid in boids)
                Debug.DrawLine(boid.transform.position, boid.transform.position + boid.transform.up * lineLength, Color.red);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireSphere(transform.position + transform.right * xBounds, 0.25f);
        Gizmos.DrawWireSphere(transform.position + -transform.right * xBounds, 0.25f);
        Gizmos.DrawWireSphere(transform.position + transform.up * yBounds, 0.25f);
        Gizmos.DrawWireSphere(transform.position + -transform.up * yBounds, 0.25f);
    }
}