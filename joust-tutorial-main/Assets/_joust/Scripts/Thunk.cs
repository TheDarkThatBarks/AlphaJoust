using System;
using System.Linq;
using UnityEngine;

public class Thunk : MonoBehaviour
{
    public event Action OnThunk = delegate { };

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer != LayerMask.NameToLayer("Ground")) return;
        if (!other.contacts.Any(contact => contact.normal.y < 0.5f)) return;
        SoundManager.Instance.PlayThudSound();
        OnThunk();
    }
}
