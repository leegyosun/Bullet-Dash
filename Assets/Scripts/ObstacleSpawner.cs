using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Prefabs (다수 입력)")]
    [SerializeField] private List<GameObject> carPrefabs;

    [Header("Timing / Position")]
    [SerializeField] private float   spawnInterval = 1.2f;         // 생성 주기(초)
    [SerializeField] private Vector2 xRange        = new(-8f, 8f); // 좌우 랜덤 범위
    [SerializeField] private float   spawnY        = 6f;           // 화면 위쪽 살짝 밖

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCar();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnCar()
    {
        if (carPrefabs.Count == 0) return;          // 방어

        float x = Random.Range(xRange.x, xRange.y); // 랜덤 X
        Vector3 pos = new(x, spawnY, 0f);

        int idx = Random.Range(0, carPrefabs.Count);
        Instantiate(carPrefabs[idx], pos, Quaternion.identity);
    }
}
