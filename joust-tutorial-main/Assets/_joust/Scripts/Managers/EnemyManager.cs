using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] ScoreManager _scoreManager;

    public event Action EnemyDestroyed = delegate { };
    public int EnemiesRemaining => _enemies.Count;

    [SerializeField] Spawner[] _spawners;
    [SerializeField] float _spawnInterval = 1f;

    float _spawnTimer;
    int _spawner;
    WaitForSeconds _spawnDelay;
    readonly List<EnemyInputManager> _enemies = new List<EnemyInputManager>();

    public void SpawnEnemies(int enemiesToSpawn)
    {
        _spawner = -1;
        _spawnDelay = new WaitForSeconds(_spawnInterval);
        StartCoroutine(SpawnEnemiesWithDelay(enemiesToSpawn));
    }

    public void RemoveEnemy(EnemyInputManager enemyInputManager)
    {
        if (!_enemies.Contains(enemyInputManager)) return;
        SoundManager.Instance.PlayKillSound();
        _scoreManager.ScoreKill();
        _enemies.Remove(enemyInputManager);
        Destroy(enemyInputManager.gameObject);
        EnemyDestroyed();
    }

    public void DestroyAllEnemies()
    {
        foreach (var enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }
        _enemies.Clear();
    }

    IEnumerator SpawnEnemiesWithDelay(int enemiesToSpawn)
    {
        while (_enemies.Count < enemiesToSpawn)
        {
            yield return _spawnDelay;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        var enemy = NextSpawner().SpawnEnemy();
        enemy.transform.SetParent(transform);
        _enemies.Add(enemy.GetComponent<EnemyInputManager>());
    }

    Spawner NextSpawner()
    {
        if (++_spawner >= _spawners.Length)
        {
            _spawner = 0;
        }

        return _spawners[_spawner];
    }
}
