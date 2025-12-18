using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, IDamageable
{

    [SerializeField] private Camera _camera;
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackDamage = 25f;
    [SerializeField] private float _attackRadius = 2f;
    [SerializeField] private float _attackAngle = 45f;

    public float Health => _health;
    private Action Attacked;

    private float _health = 100f;
    private float _lastAttackTime;

    private void OnValidate() =>
        _camera ??= Camera.main;

    private void Start() {
        SetHealth(_startHealth);
        OnDie += () => {
            Debug.Log("Player died");
            Destroy(gameObject);
        };
    }

    private void FixedUpdate() {
        if (TryAttack()) Attacked?.Invoke();
    }

    private void SetHealth(float health)
    {
        _health = health;
        OnHealthChanged?.Invoke(_health);
    }

    public event Action<float> OnHealthChanged;

    public void Damage(float damage)
    {
        if (damage < 0) throw new ArgumentException("Damage cannot be negative", nameof(damage));

        SetHealth(_health - damage);    
        if (_health <= 0) Die();
    }

    private void Die()
    {
        OnDie?.Invoke();
    }

    private bool TryAttack()
    {
        if (Time.time < _lastAttackTime + _attackCooldown) return false;
        _lastAttackTime = Time.time;

        Attack();
        return true;
    }

    private void Attack()
    {
        Vector2 mousePosition = _camera.ScreenToWorldPoint(GameManager.Input.MousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;

        foreach (Enemy enemy in EnemySpawner.Enemies)
        {
            Vector2 enemyReleativePosition = (Vector2)enemy.transform.position - (Vector2)transform.position;
            if (enemyReleativePosition.magnitude > _attackRadius
                || Vector2.Angle(attackDirection, enemyReleativePosition) > _attackAngle) continue;

            enemy.Damage(_attackDamage);
        }
    }

    public event Action OnDie;
}