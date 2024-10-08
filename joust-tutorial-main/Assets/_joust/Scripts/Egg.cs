using Unity.VisualScripting;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] float _hatchTime = 5f;
    [SerializeField] Animator _animator;

    float _hatchTimer;
    static readonly int Hatching = Animator.StringToHash("Hatching");

    void Start()
    {
        _hatchTimer = _hatchTime;
    }

    void FixedUpdate()
    {
        if (!_animator) return;
        _hatchTimer -= Time.deltaTime;
        if (_hatchTimer <= 0)
        {
            _animator.SetBool(Hatching, true);
        }
        if (transform.position.y < -10)
            Destroy(gameObject);
    }
}
