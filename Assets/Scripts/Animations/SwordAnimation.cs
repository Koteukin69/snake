using UnityEngine;
using DG.Tweening;

public class SwordAnimation : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _swordTransform;
    [SerializeField] private PlayerCombat _playerCombat;

    [SerializeField] private float _animationTime = 0.5f;

    private Tween _attackTween;

    private void OnValidate()
    {
        _camera ??= Camera.main;
        _swordTransform ??= transform;
        _playerCombat ??= GetComponentInParent<PlayerCombat>();
    }

    private void Start() =>
        _playerCombat.Attacked += PlayAttackAnimation;

    private void OnDestroy()
    {
        _attackTween?.Kill();
        _playerCombat.Attacked -= PlayAttackAnimation;
    }

    private void Update() =>
        RotateTowardsMouse();

    private void RotateTowardsMouse()
    {
        if (_attackTween != null && _attackTween.IsActive() && _attackTween.IsPlaying()) return;
        _swordTransform.eulerAngles = new Vector3(0, 0, LookAngle);
    }
    
    private void PlayAttackAnimation()
    {
        _attackTween?.Kill();

        float baseAngle = LookAngle;
        _attackTween = DOTween.Sequence()
            .Append(_swordTransform.DORotate(new Vector3(0, 0, baseAngle - _playerCombat.AttackAngle), 
                _animationTime / 4).SetEase(Ease.OutQuad))
            .Append(_swordTransform.DORotate(new Vector3(0, 0, baseAngle + _playerCombat.AttackAngle), 
                _animationTime / 2).SetEase(Ease.InOutSine))
            .Append(_swordTransform.DORotate(new Vector3(0, 0, baseAngle), 
                _animationTime / 4).SetEase(Ease.InQuad))
            .Play();
    }

    private float LookAngle => Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;
    
    private Vector2 LookDirection => 
        _camera.ScreenToWorldPoint(GameManager.Input.MousePosition) - _playerCombat.transform.position;
}
