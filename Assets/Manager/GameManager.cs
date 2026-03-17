using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum EDifficulty
{
    EASY,
    MEDIUM,
    HARD
}


public class GameManager : MonoBehaviour
{

    [Header("Scene")]
    [SerializeField] private CharacterController _player = null;
    private bool _goblinEnding = false;
    [SerializeField] private Door _door = null;

    [Header("Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _deadMusic;

    [Header("UI Values")]
    private DragHandler _dragHandler = null;
    [SerializeField] private PlayerHUDController _playerHUDController = null;
    [SerializeField] private float _soundMultiplier = 1;
    [SerializeField] private EDifficulty _difficulty = EDifficulty.MEDIUM;


    #region Properties

    public CharacterController Player { get => _player; set { _player = value; } }
    public float SoundMultiplier { get => _soundMultiplier; set => _soundMultiplier = value; }
    public bool GoblinEnding { get => _goblinEnding; set => _goblinEnding = value; }
    public Door Door { get => _door; set => _door = value; }
    public EDifficulty Difficulty { get => _difficulty; set => _difficulty = value; }
    public PlayerHUDController PlayerHUDController { get => _playerHUDController; set => _playerHUDController = value; }
    public DragHandler DragHandlerM { get => _dragHandler; set => _dragHandler = value; }

    #endregion Properties


    #region Singleton
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get => _instance;
        set => _instance = value;
    }
    #endregion Singleton


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
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (_gameLoop != null)
            _gameLoop();
    }


    public void DeadPlayer()
    {
        PlayGameManagerSouds(_deadMusic);
    }

    #region Statics

    public static void QuitGame()
    {
        Application.Quit();
    }

    public static void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    #endregion Statics


    #region Sounds
    public static void PlaySouds(AudioSource audioSource, AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.volume = GameManager.Instance.SoundMultiplier;
        audioSource.Play();
    }


    public static void StopSouds(AudioSource audioSource)
    {
        audioSource.Stop();
    }


    public void PlayGameManagerSouds(AudioClip sound)
    {
        _audioSource.clip = sound;
        _audioSource.volume = GameManager.Instance.SoundMultiplier;
        _audioSource.Play();
    }

    #endregion Sounds

}