using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField] private bool _normalEnding = true;
    [SerializeField] private FollowPath _followPath;
    [SerializeField] private EndingCat _endingCat;
    [SerializeField] private ChestBehavior _chest;
    [SerializeField] private GameObject _cat;

    [Header("Character Setup")]
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private float _lerpSpeedMultiplier = 1;
    private float _positionPerc = 0;
    private bool _moveToExit = false;

    [Header("Normal Ending")]
    [SerializeField] private string _normalEndingText;
    [SerializeField] private AudioClip _normalEndingMusic;

    [Header("Cat Ending")]
    [SerializeField] private string _catEndingText;
    [SerializeField] private AudioClip _catEndingMusic;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _catEndingSound;

    [Header("Door Setup")]
    [SerializeField] private Animation _doorAnimation;

    private void OnTriggerEnter(Collider other)
    {
        if (_normalEnding)
        {
            NormalEnding();
        }
        else
        {
            CatEnding();
        }
        Invoke("EndingScreenDelay", 5);
        Invoke("CloseDoor", 6);
        Invoke("LoadMainMenu", 8.5f);
    }

    private void Update()
    {
        if (_moveToExit)
        {
            GoToExit();
        }
    }

    private void NormalEnding()
    {
        _chest.Endings();
        GameManager.Instance.PlayerHUDController.LoreText(_normalEndingText);
        GameManager.Instance.PlayGameManagerSounds(_normalEndingMusic);
        GameManager.Instance.Player.IsMoving = false;
        GameManager.Instance.Player.transform.position = _startPosition.position;
    }


    private void CatEnding()
    {
        GameManager.PlaySounds(_audioSource, _catEndingSound); 
        GameManager.Instance.PlayerHUDController.LoreText(_catEndingText);
        GameManager.Instance.PlayGameManagerSounds(_catEndingMusic);
        _chest.SpinChest();
        _followPath.ActivateFollowPath();
        _cat.SetActive(true);
        _endingCat.ActivateCat();
        GameManager.Instance.Player.IsMoving = false;
        GameManager.Instance.Player.transform.position = _startPosition.position;
    }



    public void EndingCat()
    {
        _normalEnding = false;
        
    }

    public void EndingScreenDelay()
    {
        GameManager.Instance.PlayerHUDController.Ending(_normalEnding);
        _moveToExit = true;
    }

    private void GoToExit()
    {
        _positionPerc += Time.deltaTime * _lerpSpeedMultiplier;
        _positionPerc = Mathf.Clamp(_positionPerc, 0, 1);
        GameManager.Instance.Player.transform.position = Vector3.Lerp(_startPosition.position, _endPosition.position, _positionPerc);
    }

    private void CloseDoor()
    {
        _doorAnimation.Play();
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
