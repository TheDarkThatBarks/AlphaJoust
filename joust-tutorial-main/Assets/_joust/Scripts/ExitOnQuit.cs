using UnityEngine;

public class ExitOnQuit : MonoBehaviour
{
    PlayerInputManager _playerInputManager;
    
    void Start()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
        _playerInputManager.OnQuitPressed += QuitApplication;
    }

    void QuitApplication()
    {
        _playerInputManager.OnQuitPressed -= QuitApplication;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
