using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemyPrefabs;
    [Tooltip("Vectors of enemiesSpawnProps, where x is start wave, y is level multiplier and z is random range properties. Count of spawned enemies equals to (wave - start) * lvl_multiplier +- random_range clamped from 0."), 
    SerializeField] Vector3[] _enemiesSpawnProps = new Vector3[] {
        new Vector3(0, 15, 5),
        new Vector3(6, 10, 4),
        new Vector3(11, 5, 3),
    };
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _enemiesParent;

    [Tooltip("Range of enemies count to spawn in a group."), 
    SerializeField] private Vector2Int _groupSize = new Vector2Int(3, 7);
    [SerializeField] private float _spawnTime = 1f;

    [Tooltip("Width and height of the spawn area."), 
    SerializeField] private Vector2 _spawnArea = new Vector2(10f, 10f);
    [Tooltip("Distance from the player to the spawn area."), 
    SerializeField] private float _safeZone = 1f;
    [SerializeField] private float _enemyGroupSpread;
    [SerializeField] private float _areaIncrease = 1f;

    public static Enemy[] Enemies;

    public Transform Player => _player;
    public Vector2 SpawnArea => _spawnArea;
    public float SafeZone => _safeZone;

    private Vector2 RandomVector2 => new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

    private uint _leftEnemies;

    private void SetLeftEnemies(uint value)
    {
        _leftEnemies = value;
        LeftEnemiesChanged?.Invoke(_leftEnemies);
    }

    public Action<uint> LeftEnemiesChanged;

    private void OnValidate() =>
        _enemiesParent ??= transform;

    private void Start() {
        GameManager.WavesSystem.OnWaveEnd += _ => { IncreaseArea(); };
        GameManager.WavesSystem.OnWaveEnd += wave => StartCoroutine(SpawnCoroutine(GetEnemies(wave)));
        
        LeftEnemiesChanged += (leftEnemies) => {
            if (leftEnemies == 0) GameManager.WavesSystem.NextWave();
        };
    }

    private void IncreaseArea() {
        _spawnArea += Vector2.one * (_areaIncrease * 2);
        _safeZone += _areaIncrease;
    }

    IEnumerator SpawnCoroutine(uint[] enemies)
    {
        SetLeftEnemies(_leftEnemies + (uint) enemies.Sum(e => e));
        while (enemies.Sum(e => e) > 0)
        {
            uint enemyIndex = GetWeightedRandomIndex(enemies);
            uint spawnCount = (uint)Mathf.Clamp(Random.Range(_groupSize.x, _groupSize.y + 1), 0, (int)enemies[enemyIndex]);

            enemies[enemyIndex] -= spawnCount;
            SpawnEnemyGroup(enemyIndex, GroupSpawnPosition, _enemiesParent, spawnCount);

            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private Vector2 GroupSpawnPosition {
        get {
            Vector2 position = (Vector2) transform.position + RandomVector2 * _spawnArea * .5f;
            float distance = Vector2.Distance(position, _player.position);
            
            if (distance >= _safeZone) return position;

            position = _player.position + 
                Quaternion.LookRotation(-_player.position) * (Vector2.one * _safeZone);
            return position;
        }
    }

    private static uint GetWeightedRandomIndex(uint[] weights)
    {
        int randomValue = Random.Range(0, (int)(uint) weights.Sum(w => w));
        
        int cumulative = 0;
        for (uint i = 0; i < weights.Length; i++)
        {
            cumulative += (int) weights[i];
            if (randomValue < cumulative) return i;
        }
        
        throw new Exception("All weights are 0.");
    }

    private GameObject[] SpawnEnemyGroup(uint enemyIndex, Vector2 position, Transform parent, uint count)
    {
        return (new uint[count]).Select((_) => {
            GameObject enemyObj = SpawnEnemy(enemyIndex, position + RandomVector2 * _enemyGroupSpread, parent);
            
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            enemy?.SetTarget(_player);
            Enemies = Enemies.Append(enemy).ToArray();
            enemy.OnDie += () => {
                Enemies = Enemies.Where(e => e != enemy).ToArray();
                SetLeftEnemies(_leftEnemies - 1);
            };
            return enemyObj;
        }).ToArray();
    }

    private GameObject SpawnEnemy(uint enemyIndex, Vector3 position, Transform parent) => 
        Instantiate(_enemyPrefabs[enemyIndex], position, Quaternion.identity, parent);

    private uint[] GetEnemies(uint wave)
    {
        return _enemiesSpawnProps.Select(a => wave >= a.x ? (uint) Mathf.Clamp((wave - a.x) * a.y
            + Random.Range(-a.z, a.z + 1), 0, int.MaxValue) : 0
        ).ToArray();
    }
}
