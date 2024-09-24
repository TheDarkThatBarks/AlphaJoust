using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;
    [SerializeField] Spawner _playerSpawner;
    
    public void PlayerMountDespawned()
    {
        if (ScoreManager.Instance.Lives > 0)
        {
            _playerSpawner.SpawnPlayer();
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayStartSound();
        StartGame();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    void StartGame()
    {
        ScoreManager.Instance.ResetScore();
        SubscribeToEvents();
        StartWave();
    }

    void SubscribeToEvents()
    {
        ScoreManager.Instance.OnLivesChanged += OnLivesChanged;
        _enemyManager.EnemyDestroyed += OnEnemyDestroyed;
    }

    void UnsubscribeFromEvents()
    {
        ScoreManager.Instance.OnLivesChanged -= OnLivesChanged;
        _enemyManager.EnemyDestroyed -= OnEnemyDestroyed;
    }

    void OnLivesChanged(int lives)
    {
        if (lives < 1)
        {
            _enemyManager.DestroyAllEnemies();
            SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
            Debug.Log("Game over");
        }
    }

    void OnEnemyDestroyed()
    {
        if (_enemyManager.EnemiesRemaining < 1)
        {
            NextWave();
        }
    }

    void StartWave()
    {
        _enemyManager.SpawnEnemies(ScoreManager.Instance.Wave * 2 + 1);
    }
    
    void NextWave()
    {
        ScoreManager.Instance.NextWave();
        StartWave();
    }
}
