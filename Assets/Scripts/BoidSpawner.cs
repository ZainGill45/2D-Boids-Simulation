using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [field: Header("General Dependencies")]
    [field: SerializeField] private Transform boidPrefab;
    
    [field: Header("General Settings")]
    [field: SerializeField] private float spawnXBounds = 6f;
    [field: SerializeField] private float spawnYBounds = 4f;
    [field: SerializeField] private int boidCount = 32;

    private PolygonCollider2D boidCollider;

    private void Awake()
    {
        boidCollider = boidPrefab.GetComponent<PolygonCollider2D>();
    }
    
    private void Start()
    {
        SpawnBoids();
    }
    
    [ContextMenu("Respawn Boids")]
    public void SpawnBoids()
    {
        Boid[] boidsToDelete = FindObjectsByType<Boid>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < boidsToDelete.Length; i++)
            Destroy(boidsToDelete[i].gameObject);
        
        for (int i = 0; i < boidCount; i++)
        {
            Vector3 randomPosWithinBounds = new Vector3(
                Random.Range(-spawnXBounds, spawnXBounds), 
                Random.Range(-spawnYBounds, spawnYBounds), 
                0f);
                
            Vector3 randomEulerRotation = new Vector3(
                0f, 
                0f, 
                Random.Range(0f, 360f));

            // Wait until the position is clear for spawning.
            while (!QueryIfCanSpawnColliderAtPosition(randomPosWithinBounds))
            {
                randomPosWithinBounds = new Vector3(
                    Random.Range(-spawnXBounds, spawnXBounds), 
                    Random.Range(-spawnYBounds, spawnYBounds), 
                    0f);
            }
            
            Instantiate(boidPrefab, randomPosWithinBounds, Quaternion.Euler(randomEulerRotation));
        }
    }

    private bool QueryIfCanSpawnColliderAtPosition(Vector2 position)
    {
        // Use the boids collider bounding box size to check overlapping colliders.
        Vector2 colliderSize = boidCollider.bounds.size;
        Collider2D hitCollider = Physics2D.OverlapBox(position, colliderSize, 0f);
        return hitCollider == null;
    }
}