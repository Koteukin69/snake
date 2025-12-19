using UnityEngine;
using DG.Tweening;

public class PlayerGrowth : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    
    [SerializeField] private float _baseScale = 1f;
    [SerializeField] private float _scaleIncrement = .1f;
    [SerializeField] private float _fadeTime = .5f;

    private void OnValidate() => 
        _playerTransform ??= transform;

    private void Start() =>
        GameManager.WavesSystem.OnWaveEnd += GrowPlayer;

    private void GrowPlayer(uint waveNumber) =>
        _playerTransform.DOScale(_baseScale + (waveNumber * _scaleIncrement), _fadeTime)
            .SetEase(Ease.InOutSine).Play();
}
