using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    [field: Header("General Settings")] 
    [field: SerializeField] private float lineLength = 1f;

    public static List<Boids> boids;

    private void Awake()
    {
        boids = new List<Boids>();
    }

    private void Update()
    {
        if (boids.Count > 0)
            foreach (Boids boid in boids)
                Gizmos.DrawLine(boid.transform.position, boid.transform.position + boid.transform.forward * lineLength);
    }
}