using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : InputManagerBase, IInputManager
{
    public Vector2 Movement { get; private set; }
    public event Action<Vector2> OnMoveReceived = delegate(Vector2 vector2) {  };
    public event Action OnFlapPressed = delegate {  };
    public event Action OnQuitPressed = delegate {  };

    void OnMove(InputValue value)
    {
        Movement = value.Get<Vector2>();
        OnMoveReceived(Movement);
    }

    void OnFlap()
    {
        OnFlapPressed();
    }

    void OnQuit()
    {
        OnQuitPressed();
    }
}
