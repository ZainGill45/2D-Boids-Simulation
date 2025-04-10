using UnityEngine;

public class Boid : MonoBehaviour
{
    private void OnEnable()
    {
        DebugDrawer.boids.Add(this);
    }
    private void OnDisable()
    {
        DebugDrawer.boids.Remove(this);
    }
}
