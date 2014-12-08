using UnityEngine;
using System.Collections;

public class FollowerSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRadius = 10.0f;

    [SerializeField] private float _spawnPerMinute;
    [SerializeField] private Follower _prefab;

    public void Spawn()
    {
        Vector3 spawnPosition = transform.position;
        
        spawnPosition += new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized * Random.Range(0.0f, _spawnRadius);
        
        Instantiate(_prefab, transform.position, transform.rotation);
    }

    public void Spawn(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Spawn();
        }
    }
}
