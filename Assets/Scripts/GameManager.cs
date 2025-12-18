using UnityEngine;
using System;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private InputSystemTypes _inputSystemType;
    [SerializeField] private WavesSystem _wavesSystem;

    public static IInput Input => Instance != null ? Instance._input : throw MissingInstanceExeption;
    public static WavesSystem WavesSystem => Instance != null ? Instance._wavesSystem : throw MissingInstanceExeption;

    private IInput _input;

    private void OnValidate()
    {
        if (_wavesSystem == null) TryGetComponent(out _wavesSystem);
    }

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else throw new Exception("Only one GameManager instance is allowed.");

        _input = _inputSystemType switch
        {
            InputSystemTypes.InputSystem => new InputSystemInput(),
            InputSystemTypes.Keyboard => new KeyboardInput(),
            _ => throw new ArgumentOutOfRangeException(nameof(_inputSystemType), _inputSystemType, null),
        };
    }

    public static Exception MissingInstanceExeption => new Exception("GameManager is missing from the scene or game hasn't started yet.");
}

public enum InputSystemTypes
{
    InputSystem,
    Keyboard,
}