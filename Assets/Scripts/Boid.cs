using UnityEngine;

// Rules for boids
//  Move forward and avoid obstacles by steering away from them
//  Every frame have some percentage chance to choose a new direction
//  If another boid is nearby avoid them by giving them some amount of space
//  If another boid is nearby try to align your forward direction (-transform.up) to be aligned with theirs

public class Boid : MonoBehaviour
{
    [field: Header("General Dependencies")]
    [field: SerializeField] private Rigidbody2D physicsBody;
    
    [field: Header("General Settings")]
    [field: SerializeField] private float speed = 2f;
    [field: SerializeField] private float rotationSpeed = 360f;

    private void Update()
    {
        Debug.DrawRay(transform.position + transform.up * 0.2f, transform.up * 0.5f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up * 0.2f, transform.up, 0.5f);
        
        if (hit.collider is not null)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, transform.localEulerAngles.z + rotationSpeed * Time.deltaTime);
        }
        
        physicsBody.linearVelocity = transform.up * speed;
    }
}
