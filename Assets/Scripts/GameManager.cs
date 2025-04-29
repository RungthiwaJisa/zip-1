using NUnit.Framework;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager planeManager;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject enamyPrefab;
    Player player;

    [Header("Enemy Settings")]
    [SerializeField] private int enemyCount = 2;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float deSpawnRate = 4f;

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int score = 0;

    private bool _gameStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //var getPlayer = gameObject.Find("Player");

        UIManager.OnUIStartButton += StartGame;
        UIManager.OnUIRestartButton += RestartGame;
    }

    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;

        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            var meshVisual = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (meshVisual) meshVisual.enabled = false;

            var lineVisual = plane.GetComponent<LineRenderer>();
            if (lineVisual) lineVisual.enabled = false;
        }

        StartCoroutine(SpawnEnemies());
    }

    void RestartGame()
    {
        print("Restarted!");
        _gameStarted = false;
        arSession.Reset();
        planeManager.enabled = true;
    }


    void SpawnEnemy()
    {
        if (planeManager.trackables.count == 0) return;
        List<ARPlane> planes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            planes.Add(plane);
        }
        var randomPlane = planes[Random.Range(0, planes.Count)];
        var randomPlanePosition = GetRandomPosition(randomPlane);

        var enemy = Instantiate(enamyPrefab, randomPlanePosition, Quaternion.identity);
        _spawnedEnemies.Add(enemy);

        var enemyScript = enemy.GetComponentInChildren<EnemyScript>();
        if(enemyScript != null)
        {
            print("Run event");
            enemyScript.OnEnemyDestroyed += AddScore;
        }



        StartCoroutine(DespawnEnemies(enemy));
    }

    Vector3 GetRandomPosition(ARPlane plane)
    {
        var center = plane.center;
        var size = plane.size * 0.5f;
        var randomX = Random.Range(-size.x, size.x);
        var randomZ = Random.Range(-size.y, size.y);

        return new Vector3(center.x + randomX, center.y, center.z + randomZ);

    }


    IEnumerator SpawnEnemies()
    {
        while (_gameStarted)
        {
            if(_spawnedEnemies.Count < enemyCount)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
    IEnumerator DespawnEnemies(GameObject enemy)
    {
        EnemyScript enemyAttack = enemy.GetComponent<EnemyScript>();

        yield return new WaitForSeconds(deSpawnRate);
        if (_spawnedEnemies.Contains(enemy))
        {
            enemyAttack.AttackPlayer();

            _spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }
    void AddScore()
    {
        score++;
        
        uiManager.UpdateScore(score);
        print(score);
    }
}
