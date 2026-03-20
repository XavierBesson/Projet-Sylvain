using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuButtons : MonoBehaviour
{
    [SerializeField] private string _playSceneToLoad;


    private void Start()
    {
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(_playSceneToLoad);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}

