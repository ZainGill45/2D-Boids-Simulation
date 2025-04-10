using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer : MonoBehaviour
{
    [field: Header("General Settings")] 
    [field: SerializeField] private float lineLength = 1f;

    public static List<Boid> boids;

    private void Awake()
    {
        boids = new List<Boid>();
    }

    private void Update()
    {
        if (boids.Count > 0)
            foreach (Boid boid in boids)
                Debug.DrawLine(boid.transform.position, boid.transform.position + -boid.transform.up * lineLength, Color.red);
    }
}