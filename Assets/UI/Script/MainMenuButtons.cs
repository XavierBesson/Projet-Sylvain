using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private CanvasGroup _playMessage;
    private float _playAlpha;
    [SerializeField] private CanvasGroup _quitMessage;
    private float _quitAlpha;
    [SerializeField] private float _alphaRate = 0.2f;

    public void PlayButton()
    {
        _playAlpha = 1;
    }

    public void QuitButton()
    {
        _quitAlpha = 1;
    }

    private void Update()
    {
        _playAlpha -= Time.deltaTime * _alphaRate;
        _playAlpha = Mathf.Clamp(_playAlpha, 0, 1);
        _playMessage.alpha = _playAlpha;

        _quitAlpha -= Time.deltaTime * _alphaRate;
        _quitAlpha = Mathf.Clamp(_quitAlpha, 0, 1);
        _quitMessage.alpha = _quitAlpha;
    }
}
