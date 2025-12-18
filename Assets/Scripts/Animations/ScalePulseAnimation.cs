using UnityEngine;
using DG.Tweening;

public class ScalePulseAnimation : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _pulseScale = 1.1f;
    [SerializeField] private float _pulseTime = .5f;

    private Tween _tween;

    private void OnValidate()
    {
        if (_transform == null) _transform = transform;
    }

    private void Start() =>
        _tween = _transform.DOScaleY(_pulseScale, _pulseTime)
            .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).Play();
    
    private void OnDestroy() =>
        _tween?.Kill();
}
