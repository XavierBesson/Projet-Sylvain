using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuButtons : MonoBehaviour
{
    [SerializeField] private string _playSceneToLoad;
    [SerializeField] private TMP_Text _deathText;


    private void Start()
    {
        GameManager.Instance.DeadPlayer();
        _deathText.text = GameManager.Instance.DeathMessage;
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

