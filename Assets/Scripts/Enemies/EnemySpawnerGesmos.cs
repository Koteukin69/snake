using UnityEngine;

public class EnemySpawnerGesmos : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;

    private void OnValidate()
    {
        if (_enemySpawner == null) TryGetComponent(out _enemySpawner);
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled) return;
        if (_enemySpawner == null) throw new System.Exception("EnemySpawner isn't assigned.");

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, _enemySpawner.SpawnArea);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((_enemySpawner.Player != null) ? (Vector2)_enemySpawner.Player.position : Vector2.zero, _enemySpawner.SafeZone);
    }
}