using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] float _boundary = 6.6f;

    void FixedUpdate()
    {
        if (ShouldTeleport)
        {
            Teleport();
        }
    }

    bool ShouldTeleport => Mathf.Abs(transform.position.x) > _boundary;

    void Teleport()
    {
        var position = transform.position;
        position.x *= -0.98f;
        transform.position = position;
        transform.eulerAngles = transform.position.x > 0 ? new Vector3(0, 180, 0) : Vector3.zero;

    }
}
