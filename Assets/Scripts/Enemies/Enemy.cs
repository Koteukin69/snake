using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _startHealth = 100f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _attackCooldown = 5f;

    private float _lastAttackTime;

    public Action OnDie;
    public Action Attacked;

    protected float _health;
    private Transform _target;

    private void Start() {
        OnDie += () => Destroy(gameObject);
        _health = _startHealth;
    }

    private void FixedUpdate() {
        if (!_target) return;
        
        Move(Time.fixedDeltaTime);
        if (TryAttack()) Attacked?.Invoke();
    }

    private bool TryAttack()
    {
        if (!_target.TryGetComponent(out PlayerCombat player)) throw new Exception("Target does not have PlayerCombat component.");
        if (Vector2.Distance(transform.position, _target.position) > 1f
            || Time.time < _lastAttackTime + _attackCooldown) return false;
        _lastAttackTime = Time.time;

        Attack(player);
        return true;
    }

    private void Attack(PlayerCombat player) => player.Damage(_damage);

    public void Damage(float damage)
    {
        if (damage < 0) throw new ArgumentException("Damage cannot be negative", nameof(damage));

        _health -= damage;
        if (_health <= 0) Die();
    }

    public void SetTarget(Transform target) => _target = target;

    private void Die()
    {
        OnDie?.Invoke();
    }

    protected virtual void Move(float deltaTime) => 
        transform.Translate((_target.position - transform.position).normalized * _speed * deltaTime);
}