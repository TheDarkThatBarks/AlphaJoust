using UnityEngine;

public class MountDespawner : MonoBehaviour
{
    [SerializeField] float _boundary = 7f;

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) >= _boundary)
        {
            DespawnMount();
        }
    }

    void DespawnMount()
    {
        Debug.Log("DespawnMount()");
        if (gameObject.layer == LayerMask.NameToLayer("Player Mount"))
        {
            FindObjectOfType<GameManager>()?.PlayerMountDespawned();
        }   
        
        Destroy(gameObject);
    }
}
