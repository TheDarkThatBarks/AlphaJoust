using UnityEngine;

public class JoustResolver : MonoBehaviour
{
    [SerializeField] GameObject _mountPrefab;
    [SerializeField] Egg _eggPrefab;

    private ScoreManager _scoreManager;
    private EnemyManager _enemyManager;

    bool _dead;

    private void Start()
    {
        if(this.gameObject.layer == 3) // 3 is player layer
        {
            _enemyManager = this.transform.parent.GetChild(1).GetChild(1).GetComponent<EnemyManager>();
            _scoreManager = this.transform.parent.GetChild(1).GetChild(3).GetComponent<ScoreManager>();
        }
        if(this.gameObject.layer == 6) // common enemy
        {
            _enemyManager = GetComponentInParent<EnemyManager>();
            _scoreManager = this.transform.parent.parent.GetChild(3).GetComponent<ScoreManager>();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (_dead) return;
        var opponent = other.collider.gameObject.GetComponentInParent<JoustResolver>();
        if (!opponent) return;

        if (!LostJoust(opponent)) return;

        _dead = true;
        
        SpawnMount();
        
        if (RemovedEnemy()) return;

        _scoreManager.KillPlayer(this.gameObject);
        // Destroy(gameObject);
    }

    bool LostJoust(JoustResolver opponent)
    {
        return (opponent.transform.position.y - transform.position.y > 0.25f);
    }

    void SpawnMount()
    {
        if (_mountPrefab)
        {
            Instantiate(_mountPrefab, transform.position, transform.rotation);
        }   
    }

    bool RemovedEnemy()
    {
        if (!TryGetComponent<EnemyInputManager>(out var enemyInputManager)) return false;
        SpawnEgg();
        enemyInputManager.enabled = false;
        _enemyManager?.RemoveEnemy(enemyInputManager);
        return true;
    }

    void SpawnEgg()
    {
        if (!_eggPrefab) return;
        var egg = Instantiate(_eggPrefab, transform.position, Quaternion.identity);
        if (!egg.TryGetComponent<Rigidbody2D>(out var rb)) return;
        var forceDirection = (transform.eulerAngles == Vector3.zero) ? Vector3.right : Vector3.left;
        rb.AddForce(forceDirection * 1f, ForceMode2D.Impulse);
    }
}