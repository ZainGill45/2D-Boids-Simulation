using UnityEngine;

public class Boids : MonoBehaviour
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
