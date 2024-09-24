using System;
using UnityEngine;

public class ScoreManager : SingletonMonoBehavior<ScoreManager>
{
    public event Action<int> OnScoreChanged = delegate(int i) {  };
    public event Action<int> OnLivesChanged = delegate(int i) {  };
    
    public static int Score { get; private set; } 
    public int Lives { get; private set; }
    public int Wave { get; private set; }

    [SerializeField] EggCollectEffect _eggCollectEffectPrefab;
    [SerializeField] int _pickupValue = 250, _killValue = 500, _extraLifeInterval = 20000;

    int _enemiesKilled, _nextExtraLifeAt;

    public void ResetScore()
    {
        Wave = 0;
        Lives = 3;
        Score = 0;
        _nextExtraLifeAt = _extraLifeInterval;
        OnScoreChanged(Score);
        OnLivesChanged(Lives);
        NextWave();
    }

    public void ScorePickup(Vector3 position)
    {
        Instantiate(_eggCollectEffectPrefab, position, Quaternion.identity);
        SoundManager.Instance.PlayEggSound();
        AddPoints(_pickupValue);
    }

    public void ScoreKill()
    {
        SoundManager.Instance.PlayKillSound();
        AddPoints(++_enemiesKilled * _killValue);
    }

    public void KillPlayer(GameObject player)
    {
        SoundManager.Instance.PlayKillSound();
        Destroy(player);
        LoseLife();
    }

    public void NextWave()
    {
        ++Wave;
        _enemiesKilled = 0;
    }

    void LoseLife()
    {
        Lives--;
        OnLivesChanged(Lives);
    }

    void AddPoints(int points)
    {
        Score += points;
        OnScoreChanged(Score);
        if (Score >= _nextExtraLifeAt)
        {
            GainLife();
        }
    }

    void GainLife()
    {
        Lives++;
        _nextExtraLifeAt += _extraLifeInterval;
        OnLivesChanged(Lives);
    }
}
