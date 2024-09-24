using System.Collections;
using UnityEngine;

public class EggCollectEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _renderers;
    [SerializeField] float _fadeDuration = 2f, _fadeDelay = 1f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(_fadeDelay);
        
        var color = _renderers[0].material.color;
        while (color.a > 0.0f)
        {
            foreach (var spriteRenderer in _renderers)
            {
                var fadeSpeed = 1.0f / _fadeDuration;
                color.a -= fadeSpeed * Time.deltaTime;
                spriteRenderer.material.color = color;
                transform.position += Vector3.up * Time.deltaTime;
                transform.position += Vector3.right * Time.deltaTime;
                yield return null;
            }
        }
        
        Destroy(gameObject);
    }
}
