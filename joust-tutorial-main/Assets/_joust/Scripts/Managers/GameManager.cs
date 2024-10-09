using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] EnemyManager _enemyManager;
    [SerializeField] public Spawner _playerSpawner;
    [SerializeField] ScoreManager _scoreManager;
    
    public void PlayerMountDespawned()
    {
        if (_scoreManager.Lives > 0)
        {
            _playerSpawner.SpawnPlayer();
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayStartSound();
        //StartGame();
    }

    void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void StartGame()
    {
        _scoreManager.ResetScore();
        SubscribeToEvents();
        StartWave();
    }

    void SubscribeToEvents()
    {
        _scoreManager.OnLivesChanged += OnLivesChanged;
        _enemyManager.EnemyDestroyed += OnEnemyDestroyed;
        Debug.Log("SubscribeToEvents");
    }

    public void UnsubscribeFromEvents()
    {
        _scoreManager.OnLivesChanged -= OnLivesChanged;
        _enemyManager.EnemyDestroyed -= OnEnemyDestroyed;
    }

    void OnLivesChanged(int lives)
    {
        if (lives < 1)
        {
            _enemyManager.DestroyAllEnemies();
            // SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
            Debug.Log("Game over");
        }
    }

    void OnEnemyDestroyed()
    {
        Debug.Log("OnEnemyDestroyed");
        if (_enemyManager.EnemiesRemaining < 1)
        {
            NextWave();
        }
    }

    void StartWave()
    {
        Debug.Log("Enemies spawned: " + Mathf.Min(_scoreManager.Wave * 2 + 1, 10));
        _enemyManager.SpawnEnemies(Mathf.Min(_scoreManager.Wave * 2 + 1, 10));
    }
    
    void NextWave()
    {
        Debug.Log("Game Manager Next Wave");
        _scoreManager.NextWave();
        StartWave();
    }
}
