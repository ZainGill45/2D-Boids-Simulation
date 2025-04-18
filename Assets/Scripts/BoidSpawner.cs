using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [field: Header("General Dependencies")]
    [field: SerializeField] private Camera mainCamera;
    [field: SerializeField] private Transform boidPrefab;
    
    [field: Header("General Settings")]
    [field: SerializeField] private float spawnXBounds = 8.25f;
    [field: SerializeField] private float spawnYBounds = 4.35f;
    [field: SerializeField] private int boidCount = 32;

    private PolygonCollider2D boidCollider;

    private void Awake()
    {
        boidCollider = boidPrefab.GetComponent<PolygonCollider2D>();
    }
    private void Start()
    {
        SpawnBoids(boidCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Spawn a boid at the where ever the user clicked the mouse
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(boidPrefab, mousePosition, Quaternion.identity);
        }
    }

    private void SpawnBoids(int numberOfBoidsToSpawn)
    {
        if (numberOfBoidsToSpawn <= 0)
            return;
        
        Boid[] boidsToDelete = FindObjectsByType<Boid>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < boidsToDelete.Length; i++)
            Destroy(boidsToDelete[i].gameObject);
        
        for (int i = 0; i < numberOfBoidsToSpawn; i++)
        {
            Vector3 randomPosWithinBounds = new(Random.Range(-spawnXBounds, spawnXBounds), Random.Range(-spawnYBounds, spawnYBounds), 0f);
            Vector3 randomEulerRotation = new(0f, 0f, Random.Range(0f, 360f));

            while (!QueryIfCanSpawnColliderAtPosition(randomPosWithinBounds))
                randomPosWithinBounds = new Vector3(Random.Range(-spawnXBounds, spawnXBounds), Random.Range(-spawnYBounds, spawnYBounds), 0f);
            
            Instantiate(boidPrefab, randomPosWithinBounds, Quaternion.Euler(randomEulerRotation));
        }
    }

    private bool QueryIfCanSpawnColliderAtPosition(Vector2 position)
    {
        Vector2 colliderSize = boidCollider.bounds.size;
        Collider2D hitCollider = Physics2D.OverlapBox(position, colliderSize, 0f);
        return hitCollider == null;
    }
}