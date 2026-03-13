using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _tutoCanvas;
    [SerializeField] private string _playSceneToLoad;


    private void Start()
    {
        DisplayMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(_playSceneToLoad);
    }


    public void DisplayMainMenu()
    {
        _mainCanvas.enabled = true;
        _tutoCanvas.enabled = false;
    }


    public void DisplayTuto()
    {
        _mainCanvas.enabled = false;
        _tutoCanvas.enabled = true;
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
