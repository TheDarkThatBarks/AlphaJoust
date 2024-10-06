using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] ScoreManager _scoreManager;

    [SerializeField] TMP_Text _score;
    [SerializeField] Transform _playerLivesContainer;
    [SerializeField] GameObject _playerLifePrefab;

    void Start()
    {
        _scoreManager.OnScoreChanged += OnScoreChanged;
        _scoreManager.OnLivesChanged += OnLivesChanged;
    }

    void OnDisable()
    {
        _scoreManager.OnScoreChanged -= OnScoreChanged;
        _scoreManager.OnLivesChanged -= OnLivesChanged;
        RemoveExtraPlayerLifeIcons(0);
    }

    int RemoveExtraPlayerLifeIcons(int lives)
    {
        int childCount = _playerLivesContainer.childCount;
        while (childCount > lives)
        {
            Destroy(_playerLivesContainer.GetChild(--childCount).gameObject);
        }

        return childCount;
    }

    void AddMissingPlayerLifeIcons(int lives, int childCount)
    {
        while (childCount < lives)
        {
            Instantiate(_playerLifePrefab, _playerLivesContainer);
            ++childCount;
        }
    }

    void OnScoreChanged(int score)
    {
        _score.text = score.ToString();
    }

    void OnLivesChanged(int lives)
    {
        var childCount = RemoveExtraPlayerLifeIcons(lives);
        AddMissingPlayerLifeIcons(lives, childCount);
    }
}
