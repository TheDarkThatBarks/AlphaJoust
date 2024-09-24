using UnityEngine;

public class Hatch : MonoBehaviour
{
    [SerializeField] GameObject _enemyKnightPrefab;

    public void HatchKnight()
    {
        Instantiate(_enemyKnightPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
