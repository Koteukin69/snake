using UnityEngine;
using TMPro;
using DG.Tweening;

public class WavesUI : MonoBehaviour {
    [SerializeField] private CanvasGroup _panel;
    [SerializeField] private TextMeshProUGUI _waveText;
    
    [SerializeField] private float _fadeTime = .5f;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private string _waveTextFormat = "Wave {0}";

    private void OnValidate()
    {
        if (_panel == null) TryGetComponent(out _panel);
        _waveText ??= GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        if (_panel) _panel.alpha = 0;
        GameManager.WavesSystem.OnWaveStart += WaveStart;
    }

    private void WaveStart(uint wave)
    {
        _waveText.SetText(string.Format(_waveTextFormat, wave));

        DOTween.Sequence().Append(
            _panel?.DOFade(1, _fadeTime).SetEase(Ease.InOutSine)
        ).AppendInterval(_delayTime).SetLoops(2, LoopType.Yoyo).Play();
    }
}