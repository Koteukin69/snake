using UnityEngine;

public class KeyboardInput : IInput
{
    public Vector2 Movement => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
}
