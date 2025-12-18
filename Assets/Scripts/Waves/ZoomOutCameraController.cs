using UnityEngine;
using DG.Tweening;

public class ZoomOutCameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    [SerializeField] private float _baseZoom = 5f;
    [SerializeField] private float _zoomIncrement = 1f;
    [SerializeField] private float _fadeTime = .5f;

    private void OnValidate()
    {
        if (_camera == null) TryGetComponent(out _camera);
    }

    private void Start()
    {
        GameManager.WavesSystem.OnWaveEnd += ZoomOut;
    }

    private void ZoomOut(uint waveNumber)
    {
        _camera.DOOrthoSize(_baseZoom + (waveNumber * _zoomIncrement), _fadeTime).SetEase(Ease.InOutSine).Play();
    }
}
