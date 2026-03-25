using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingMenuButtons : MonoBehaviour
{
    [SerializeField] private string _playSceneToLoad;
    [SerializeField] private TMP_Text _endingText;
    [SerializeField] private string _displayedText = string.Empty;


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

