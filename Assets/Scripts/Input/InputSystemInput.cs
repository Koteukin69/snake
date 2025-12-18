using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemInput : IInput
{
    public Vector2 Movement => InputSystem.actions.FindAction("Move").ReadValue<Vector2>().normalized;
    public Vector2 MousePosition => InputSystem.actions.FindAction("MousePosition").ReadValue<Vector2>();
}
