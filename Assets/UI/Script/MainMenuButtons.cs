using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private DragHandler _dragHandler;
    [Header("Play button")]
    [SerializeField] private CanvasGroup _playMessage;
    [SerializeField] private string _playSceneToLoad;
    private float _playAlpha;
    [Header("Quit button")]
    [SerializeField] private CanvasGroup _quitMessage;
    private float _quitAlpha;
    [Header("Message Settings")]
    [SerializeField] private float _alphaRate = 0.2f;

    private void Update()
    {
        _playAlpha -= Time.deltaTime * _alphaRate;
        _playAlpha = Mathf.Clamp(_playAlpha, 0, 1);
        _playMessage.alpha = _playAlpha;

        _quitAlpha -= Time.deltaTime * _alphaRate;
        _quitAlpha = Mathf.Clamp(_quitAlpha, 0, 1);
        _quitMessage.alpha = _quitAlpha;
    }

    public void PlayButton()
    {
        _playAlpha = 1;
    }

    public void QuitButton()
    {
        _quitAlpha = 1;
    }

    public void PlayGame()
    {
        if (_dragHandler != null && !_dragHandler.UiElement.Attached)
            SceneManager.LoadScene(_playSceneToLoad);
    }

    public void QuitGame()
    {
        if (_dragHandler != null && !_dragHandler.UiElement.Attached)
            Application.Quit();
    }
}
