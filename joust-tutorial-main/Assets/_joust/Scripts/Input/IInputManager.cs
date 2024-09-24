using System;
using UnityEngine;

public interface IInputManager
{   
    Vector2 Movement { get; }
    event Action<Vector2> OnMoveReceived;
    event Action OnFlapPressed;
}