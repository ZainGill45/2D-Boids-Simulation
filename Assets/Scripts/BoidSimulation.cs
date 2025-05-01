using System.Collections.Generic;
using UnityEngine;

public class BoidSimulation : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Sprite boidSprite;

    [Header("General Settings")]
    [SerializeField] private int numberOfBoids = 32;
    [SerializeField, Range(1f, 10f)] private int arenaXBounds = 8;
    [SerializeField, Range(1f, 5f)] private int arenaYBounds = 4;

    private List<Transform> boids;

    private void Awake()
    {
        for (int i = 0; i < numberOfBoids; i++)
        {
            GameObject boid = new("Boid");
            SpriteRenderer sprite = boid.AddComponent<SpriteRenderer>();
            sprite.sprite = boidSprite;

            boid.transform.localScale = new Vector3(0.3f, 0.45f, 0.3f);
            boid.transform.position = new Vector3(Random.Range(-arenaXBounds, arenaXBounds), Random.Range(-arenaYBounds, arenaYBounds), 0);
            boid.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

            boids.Add(boid.transform);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(arenaXBounds * 2f, arenaYBounds * 2f));
    }
}