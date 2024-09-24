using UnityEngine;

public class InputManagerBase : MonoBehaviour
{
    IInputManager _inputManager;

    public IInputManager InputManager
    {
        get { return _inputManager ??= GetComponent<IInputManager>(); }
    }
}