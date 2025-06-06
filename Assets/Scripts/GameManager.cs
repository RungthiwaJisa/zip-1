using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private float timeComplete = 90f;

    [Header("AR Components")]
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARPlaneManager planeManager;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject[] enamyPrefab;
    [SerializeField] private Player player;

    [Header("Enemy Settings")]
    [SerializeField] private int enemyCount = 6;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float deSpawnRate = 4f;

    private float times;
    public bool win;

    private List<GameObject> _spawnedEnemies = new List<GameObject>();
    private int score = 0;
    private bool _gameStarted;

    void Start()
    {
        if (player == null)
        {
            var playerGO = GameObject.Find("Player");
            if (playerGO != null)
            {
                player = playerGO.GetComponent<Player>();
            }
        }

        UIManager.OnUIStartButton += StartGame;
        UIManager.OnUIRestartButton += RestartGame;
    }

    private void FixedUpdate()
    {
        if (_gameStarted) 
        {
            times += Time.deltaTime;



            if (times > timeComplete)
            {
                Win();
            }
            else
            {
                uiManager.UpdateTime(times);
            }
        }
        
    }

    void StartGame()
    {
        if (_gameStarted) return;
        _gameStarted = true;

        times = 0;
        player.hp = 100;

        uiManager.UpdateTime(times);
        uiManager.UpdateHP();

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
    public void RestartGame()
    {
        Debug.Log("Restarted!");
        _gameStarted = false;

        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        _spawnedEnemies.Clear();
        score = 0;

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }

        uiManager.LobbyWindow();

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

        int randomPrefab = Random.Range(0, enamyPrefab.Length);

        var enemy = Instantiate(enamyPrefab[randomPrefab], randomPlanePosition, Quaternion.identity);
        _spawnedEnemies.Add(enemy);

        var enemyScript = enemy.GetComponent<EnemyScript>();
        if (enemyScript == null)
        {
            enemyScript = enemy.GetComponentInChildren<EnemyScript>();
        }

        if (enemyScript != null)
        {
            Debug.Log("Run event");
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
            if (_spawnedEnemies.Count < enemyCount)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator DespawnEnemies(GameObject enemy)
    {
        yield return new WaitForSeconds(deSpawnRate);

        if (enemy == null) yield break;

        if (_spawnedEnemies.Contains(enemy))
        {
            EnemyScript enemyAttack = enemy.GetComponent<EnemyScript>();
            if (enemyAttack == null)
            {
                enemyAttack = enemy.GetComponentInChildren<EnemyScript>();
            }

            if (enemyAttack != null)
            {
                enemyAttack.AttackPlayer();
            }

            _spawnedEnemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    void AddScore()
    {
        score++;

        _spawnedEnemies.RemoveAt(0);

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }
        Debug.Log("Score: " + score);
    }

    void Win()
    {
        win = true;
        _gameStarted = false;

        foreach (var enemy in _spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        _spawnedEnemies.Clear();

        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
        }

        uiManager.LobbyWindow();
    }
}