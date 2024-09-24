using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] InputManagerBase _inputMgr;

    void OnEnable()
    {
        _inputMgr.InputManager.OnFlapPressed += OnFlapPressed;
    }

    void OnDisable()
    {
        _inputMgr.InputManager.OnFlapPressed -= OnFlapPressed;
    }

    void OnFlapPressed()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}


