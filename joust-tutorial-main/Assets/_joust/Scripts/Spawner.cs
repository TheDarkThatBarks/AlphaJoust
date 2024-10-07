using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] BirdMover _playerPrefab, _enemyPrefab;
    [SerializeField] AudioClip _spawnPlayerSound, _spawnEnemySound;
    [SerializeField] SpriteRenderer _renderer;

    [ContextMenu("Spawn Player")]
    public void SpawnPlayer()
    {
        Debug.Log("Spawning player");
        SoundManager.Instance.PlayAudioClip(_spawnPlayerSound);
        // Destroy player method: var player = Instantiate(_playerPrefab, transform.position, Quaternion.identity);
        // Deactivate, move, and activate method:
        var player = _playerPrefab;
        player.transform.position = transform.position;
        player.gameObject.SetActive(true);
        player.Init(Random.Range(0f, 100f) > 50f ? Vector3.right : Vector3.left);
        transform.localScale *= 0.1f;
        StartCoroutine(player.ShowSpawnEffect(_spawnPlayerSound.length));
        StartCoroutine(ShowSpawnEffect(_spawnPlayerSound.length));
    }
    
    [ContextMenu("Spawn Enemy")]
    public GameObject SpawnEnemy()
    {
        Debug.Log("Spawning enemy");
        SoundManager.Instance.PlayAudioClip(_spawnEnemySound);
        var enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        enemy.Init(Random.Range(0f, 100f) > 50f ? Vector3.right : Vector3.left);
        transform.localScale *= 0.1f;
        StartCoroutine(enemy.ShowSpawnEffect(_spawnEnemySound.length));
        StartCoroutine(ShowSpawnEffect(_spawnEnemySound.length));
        return enemy.gameObject;
    }

    IEnumerator ShowSpawnEffect(float spawnLength)
    {
        float time = spawnLength;
        while (time >= 0f)
        {
            time -= Time.deltaTime;

            _renderer.material.color = Random.ColorHSV();
            yield return null;
        }

        _renderer.material.color = Color.white;
    }
}
