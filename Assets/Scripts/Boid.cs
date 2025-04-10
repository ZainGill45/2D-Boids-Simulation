using UnityEngine;

// Rules for boids
//  Move forward and avoid obstacles by steering away from them
//  Every frame have some percentage chance to choose a new direction
//  If another boid is nearby avoid them by giving them some amount of space
//  If another boid is nearby try to align your forward direction (-transform.up) to be aligned with theirs

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
