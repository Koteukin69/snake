using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Min(.001f)] float _speed = 5f;
    [SerializeField] private Rigidbody2D _rb;

    private void OnValidate()
    {
        if (_rb == null) TryGetComponent(out _rb);
    }

    private void FixedUpdate()
    {
        Vector2 movement = GameManager.Input.Movement * (_speed * Time.fixedDeltaTime);
        
        _rb.MovePosition(_rb.position + movement);
    }
}
