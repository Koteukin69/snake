using System;
using UnityEngine;

public class WavesSystem : MonoBehaviour
{
    public Action<uint> OnWaveStart;
    public Action<uint> OnWaveEnd;
    
    private uint _currentWave;

    private void Start() => NextWave();

    public void NextWave()
    {
        EndWave();
        StartWave();
    }

    public void EndWave()
    {
        _currentWave++;
        OnWaveEnd?.Invoke(_currentWave);
    }

    public void StartWave()
    {
        OnWaveStart?.Invoke(_currentWave);
    }
}