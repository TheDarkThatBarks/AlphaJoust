using UnityEngine;

[RequireComponent(typeof(AudioSource))]
class SoundManager : SingletonMonoBehavior<SoundManager>
{
    [SerializeField] AudioClip _flapSound, _thudSound, _skidSound, _runSound;
    [SerializeField] AudioClip _eggSound, _startSound, _killSound, _pterodactylSound;
    [SerializeField] AudioClip _freeManSound;
    
    AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayAudioClip(AudioClip sound, float volume = 1f)
    {
        _audioSource.PlayOneShot(sound, volume);
    }

    public void PlayFlapSound()
    {
        _audioSource.PlayOneShot(_flapSound);
    }

    public void PlayRunSound()
    {
        _audioSource.PlayOneShot(_runSound, 0.5f);
    }

    public void PlaySkidSound()
    {
        _audioSource.PlayOneShot(_skidSound, 0.5f);
    }

    public void PlayThudSound()
    {
        _audioSource.PlayOneShot(_thudSound, 0.5f);
    }
    
    public void PlayEggSound()
    {
        _audioSource.PlayOneShot(_eggSound);
    }
    
    public void PlayStartSound()
    {
        _audioSource.PlayOneShot(_startSound);
    }
    
    public void PlayKillSound()
    {
        _audioSource.PlayOneShot(_killSound);
    }
    
    public void PlayPterodactylSound()
    {
        _audioSource.PlayOneShot(_pterodactylSound);
    }
    
    public void PlayFreeManSound()
    {
        _audioSource.PlayOneShot(_freeManSound);
    }
    
}