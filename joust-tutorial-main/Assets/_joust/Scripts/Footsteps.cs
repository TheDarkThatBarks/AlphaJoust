using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public void PlayFootstepSound()
    {
        SoundManager.Instance.PlayRunSound();
    }
}
