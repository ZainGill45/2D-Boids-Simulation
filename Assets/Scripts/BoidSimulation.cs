using System.Collections.Generic;
using UnityEngine;

public class BoidSimulation : MonoBehaviour
{
    private class BoidData
    {
        public Transform transform;
        public Vector3 velocity;
        public Vector3 acceleration;

        public BoidData(Transform transform, Vector3 velocity)
        {
            this.transform = transform;
            this.velocity = velocity;
            acceleration = Vector3.zero;
        }
    }

    [Header("Dependencies")]
    [SerializeField] private Sprite boidSprite;


    [Header("General Settings")]
    [SerializeField] private int numberOfBoids = 50;
    [SerializeField] private float arenaXBounds = 8.8f;
    [SerializeField] private float arenaYBounds = 5f;

    [Header("Boid Settings")]
    [Range(0.1f, 10f)] public float minSpeed = 2f;
    [Range(1f, 20f)] public float maxSpeed = 5f;
    [Range(0.1f, 10f)] public float perceptionRadius = 2.5f;
    [Range(0.1f, 5f)] public float separationRadius = 1.0f;
    [Range(0.1f, 10f)] public float maxSteerForce = 3f;

    [Header("Rule Weights")]
    [Range(0f, 5f)] public float separationWeight = 1.5f;
    [Range(0f, 5f)] public float alignmentWeight = 1.0f;
    [Range(0f, 5f)] public float cohesionWeight = 1.0f;
    [Range(0f, 5f)] public float boundaryWeight = 2.0f;


    private List<BoidData> boids;

    private List<BoidData> neighborCache = new List<BoidData>();

    private void Awake()
    {
        boids = new List<BoidData>(numberOfBoids);

        for (int i = 0; i < numberOfBoids; i++)
        {

            GameObject boidGO = new("Boid_" + i);
            boidGO.transform.SetParent(transform);

            SpriteRenderer sprite = boidGO.AddComponent<SpriteRenderer>();
            sprite.sprite = boidSprite;
            sprite.sortingOrder = 1;
            boidGO.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);

            Vector3 initialPosition = new Vector3(Random.Range(-arenaXBounds, arenaXBounds), Random.Range(-arenaYBounds, arenaYBounds), 0);
            Quaternion initialRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            boidGO.transform.SetPositionAndRotation(initialPosition, initialRotation);

            Vector3 initialVelocity = boidGO.transform.up * (minSpeed + maxSpeed) * 0.5f;

            BoidData newBoidData = new BoidData(boidGO.transform, initialVelocity);
            boids.Add(newBoidData);
        }
    }

    private void Update()
    {
        if (boids == null || boids.Count == 0) return;

        foreach (BoidData currentBoid in boids)
        {
            currentBoid.acceleration = Vector3.zero;

            neighborCache.Clear();
            FindNeighbors(currentBoid, neighborCache);

            if (neighborCache.Count > 0)
            {
                Vector3 separationForce = CalculateSeparation(currentBoid, neighborCache) * separationWeight;
                Vector3 alignmentForce = CalculateAlignment(currentBoid, neighborCache) * alignmentWeight;
                Vector3 cohesionForce = CalculateCohesion(currentBoid, neighborCache) * cohesionWeight;

                currentBoid.acceleration += separationForce;
                currentBoid.acceleration += alignmentForce;
                currentBoid.acceleration += cohesionForce;
            }

            Vector3 boundaryForce = CalculateBoundarySteering(currentBoid) * boundaryWeight;
            currentBoid.acceleration += boundaryForce;
        }


        foreach (BoidData currentBoid in boids)
        {
            currentBoid.velocity += currentBoid.acceleration * Time.deltaTime;

            float speed = currentBoid.velocity.magnitude;
            Vector3 direction = currentBoid.velocity / speed;
            speed = Mathf.Clamp(speed, minSpeed, maxSpeed);
            currentBoid.velocity = direction * speed;

            currentBoid.transform.position += currentBoid.velocity * Time.deltaTime;

            if (currentBoid.velocity != Vector3.zero)
            {
                currentBoid.transform.up = currentBoid.velocity.normalized;
            }

            WrapAroundBounds(currentBoid);
        }
    }

    private void FindNeighbors(BoidData currentBoid, List<BoidData> neighborsOutput)
    {
        foreach (BoidData otherBoid in boids)
        {
            if (otherBoid == currentBoid) continue;

            Vector3 offset = otherBoid.transform.position - currentBoid.transform.position;
            float sqrDist = offset.sqrMagnitude;

            if (sqrDist < perceptionRadius * perceptionRadius)
            {
                neighborsOutput.Add(otherBoid);
            }
        }
    }

    private Vector3 CalculateSeparation(BoidData currentBoid, List<BoidData> neighbors)
    {
        Vector3 separationMove = Vector3.zero;
        int neighborsInSeparationRadius = 0;

        foreach (BoidData other in neighbors)
        {
            Vector3 offset = currentBoid.transform.position - other.transform.position;
            float distanceSqr = offset.sqrMagnitude;

            if (distanceSqr > 0 && distanceSqr < separationRadius * separationRadius)
            {
                separationMove += offset.normalized / Mathf.Sqrt(distanceSqr);
                neighborsInSeparationRadius++;
            }
        }

        if (neighborsInSeparationRadius > 0)
        {
            separationMove /= neighborsInSeparationRadius;
        }

        if (separationMove.magnitude > 0)
        {
            Vector3 desiredVelocity = separationMove.normalized * maxSpeed;
            return SteerTowards(currentBoid, desiredVelocity);
        }
        return Vector3.zero;
    }

    private Vector3 CalculateAlignment(BoidData currentBoid, List<BoidData> neighbors)
    {
        Vector3 averageVelocity = Vector3.zero;

        foreach (BoidData other in neighbors)
        {
            averageVelocity += other.velocity;
        }

        if (neighbors.Count > 0)
        {
            averageVelocity /= neighbors.Count;
            Vector3 desiredVelocity = averageVelocity.normalized * maxSpeed;
            return SteerTowards(currentBoid, desiredVelocity);
        }
        return Vector3.zero;
    }

    private Vector3 CalculateCohesion(BoidData currentBoid, List<BoidData> neighbors)
    {
        Vector3 centerOfMass = Vector3.zero;

        foreach (BoidData other in neighbors)
        {
            centerOfMass += other.transform.position;
        }

        if (neighbors.Count > 0)
        {
            centerOfMass /= neighbors.Count;
            Vector3 directionToCenter = centerOfMass - currentBoid.transform.position;
            Vector3 desiredVelocity = directionToCenter.normalized * maxSpeed;
            return SteerTowards(currentBoid, desiredVelocity);
        }
        return Vector3.zero;
    }

    private Vector3 SteerTowards(BoidData boid, Vector3 desiredVelocity)
    {
        Vector3 steer = desiredVelocity - boid.velocity;
        steer = Vector3.ClampMagnitude(steer, maxSteerForce);
        return steer;
    }

    private void WrapAroundBounds(BoidData boid)
    {
        Vector3 position = boid.transform.position;
        float buffer = 0.5f;

        if (position.x > arenaXBounds + buffer) position.x = -arenaXBounds - buffer;
        if (position.x < -arenaXBounds - buffer) position.x = arenaXBounds + buffer;
        if (position.y > arenaYBounds + buffer) position.y = -arenaYBounds - buffer;
        if (position.y < -arenaYBounds - buffer) position.y = arenaYBounds + buffer;

        boid.transform.position = position;
    }

    private Vector3 CalculateBoundarySteering(BoidData boid)
    {
        Vector3 position = boid.transform.position;
        Vector3 desired = Vector3.zero;

        float checkRadius = Mathf.Max(arenaXBounds, arenaYBounds) * 0.9f;

        if (position.magnitude > checkRadius)
        {
            desired = -position.normalized * maxSpeed;
            return SteerTowards(boid, desired);

        }

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(arenaXBounds * 2f, arenaYBounds * 2f, 0));
    }
}