using System;
using UnityEngine;

public class EggCollector : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"EggCollector {name} collided with {other.collider.gameObject.name}");
        if (IsEgg(other, out var egg))
        {
            Collect(egg);
        }
        else if (IsKnight(other, out var knight))
        {
            Collect(knight);
        }
        
    }

    bool IsKnight(Collision2D other, out EnemyKnight knight)
    {
        knight = other.collider.gameObject.GetComponentInParent<EnemyKnight>();
        return knight;
    }

    bool IsEgg(Collision2D other, out Egg egg)
    {
        egg = other.collider.gameObject.GetComponentInParent<Egg>();
        return egg;
    }

    void Collect(Component pickup)
    {
        ScoreManager.Instance.ScorePickup(pickup.transform.position);
        Destroy(pickup.gameObject);
    }
}