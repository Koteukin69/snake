using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerCombat _playerCombat;

    private void OnValidate() {
        if (_playerCombat == null) TryGetComponent(out _playerCombat);
    }

    private void Start() =>
        _playerCombat.OnHealthChanged += UpdateHealth;

    private void UpdateHealth(float health) => Debug.Log($"Health: {health}");
}
