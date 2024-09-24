using UnityEngine;

public class SingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != GetComponent<T>())
        {
            Destroy(gameObject);
            return;
        }

        Instance = GetComponent<T>();
    }

}