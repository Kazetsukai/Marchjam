using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] float SpawnRate = 0.2f;

    float elapsed;

    void Start()
    {

    }
    
    void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed > (1f/ SpawnRate))
        {
            elapsed = 0;
            GameObject newEnemy = (GameObject) GameObject.Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        }
    }
}
