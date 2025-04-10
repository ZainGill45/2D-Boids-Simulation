using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [field: Header("General Dependencies")]
    [field: SerializeField] private Transform boidPrefab;
    
    [field: Header("General Settings")]
    [field: SerializeField] private int boidCount = 20;
    [field: SerializeField] private float spawnXBounds = 6f;
    [field: SerializeField] private float spawnYBounds = 4f;

    private void Start()
    {
        for (int i = 0; i < boidCount; i++)
        {
            float x = Random.Range(-spawnXBounds, spawnXBounds);
            float y = Random.Range(-spawnYBounds, spawnYBounds);
            
            Vector3 randomPosWithinBounds = new(x, y, 0f);
            
            Vector3 randomEulerRotation = new(0f, 0f, Random.Range(0f, 360f));
            
            Instantiate(boidPrefab, randomPosWithinBounds, Quaternion.Euler(randomEulerRotation));
        }
    }
}