using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Scene")]
    [SerializeField] private CharacterController _player = null;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _timerAudioSource;

    [SerializeField] private SceneAsset _mainMenuScene;

    public CharacterController Player { get => _player; set { _player = value; } }


    #region Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }
    #endregion Singleton


    #region Properties


    #endregion Properties


    #region Event
    private event Action _gameLoop = null;
    public event Action GameLoop
    {
        add
        {
            _gameLoop -= value;
            _gameLoop += value;
        }
        remove
        {
            _gameLoop -= value;
        }
    }
    #endregion Event


    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (_gameLoop != null)
            _gameLoop();
    }






    #region Statics

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void LoadLevel(SceneAsset level)
    {
        SceneManager.LoadScene(level.name);
    }

    #endregion Statics


    #region Sounds
    public static void PlaySouds(AudioSource audioSource, AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }


    public static void StopSouds(AudioSource audioSource)
    {
        audioSource.Stop();
    }


    public void PlayGameManagerSouds(AudioClip sound)
    {
        _audioSource.clip = sound;
        _audioSource.Play();
    }

    #endregion Sounds

}