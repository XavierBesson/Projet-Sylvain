using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class Enemy : EnigmeObject
{
    [Header("HP")]
    [SerializeField] private int _maxHp = 3;
    private int _currentHp;

    [Header("Souds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private AudioClip _fuiteSound;
    [SerializeField] private AudioClip _fuiteCancelSound;

    [Header("Path")]
    [SerializeField] private GameObject _deathplacment;
    [SerializeField] private FollowPath _followPath;
    [SerializeField] private Ending _endingObject;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _easyBlood;
    [SerializeField] private ParticleSystem _mediumBlood;
    [SerializeField] private ParticleSystem _hardBlood;
    [SerializeField] private GameObject _bloodPlacement;

    [Header("Body")]
    [SerializeField] private GameObject _body1;
    [SerializeField]  private GameObject _body2;
    [SerializeField] private GameObject _body3;
    [SerializeField] private GameObject _easySword;
    [SerializeField] private GameObject _mediumSword;
    [SerializeField] private GameObject _hardSword;

    [Header("Text")]
    [SerializeField] private string _appearText = "";

    [Header("EasySwordStats")]
    [SerializeField] private int _easySwordAtk = 1;
    [SerializeField] private float _easySwordVelocity = 0.1f;
    [SerializeField] private string _easySwordText;

    [Header("EasySwordStats")]
    [SerializeField] private int _mediumSwordAtk = 5;
    [SerializeField] private float _mediumSwordVelocity = 2f;
    [SerializeField] private string _mediumSwordText;

    [Header("EasySwordStats")]
    [SerializeField] private int _hardSwordAtk = 100;
    [SerializeField] private float _hardSwordVelocity = 5f;
    [SerializeField] private string _hardSwordText;




    void Start()
    {
        _currentHp = _maxHp;
        _body1.gameObject.SetActive(true);
        _body2.gameObject.SetActive(false);
        _body3.gameObject.SetActive(false);
        _easySword.gameObject.SetActive(false);
        _mediumSword.gameObject.SetActive(false);
        _hardSword.gameObject.SetActive(false);
    }


    public void TakeDamage(int damage, UIObject sword)
    {
        _currentHp -= damage;
        PlaySound(_audioSource, _damageSound);

        if (_currentHp <= 0)
            Die(sword);

        switch (GameManager.Instance.Difficulty)
        {
            case EDifficulty.EASY:
                _easyBlood = Instantiate(_easyBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
                _easyBlood.Play();
                break;
            case EDifficulty.MEDIUM:
                _mediumBlood = Instantiate(_mediumBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
                _mediumBlood.Play();
                break;
            case EDifficulty.HARD:
                _hardBlood = Instantiate(_hardBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
                _hardBlood.Play();
                break;
        }
    }


    private void Die(UIObject sword)
    {
        PlaySound(_audioSource, _dieSound);
        sword.Despawn();

        _body1.gameObject.SetActive(false);
        _body2.gameObject.SetActive(true);
        _body3.gameObject.SetActive(true);

        switch (GameManager.Instance.Difficulty)
        {
            case EDifficulty.EASY:
                _easySword.gameObject.SetActive(true);
                break;
            case EDifficulty.MEDIUM:
                _mediumSword.gameObject.SetActive(true);
                break;
            case EDifficulty.HARD:
                _hardSword.gameObject.SetActive(true);
                break;
        }

        Destroy(GetComponent<BoxCollider>());
    }


    private void Fuite()
    {
        PlaySound(_audioSource, _fuiteSound);
        _endingObject.EndingCat();
        GameManager.Instance.GoblinEnding = true;
        GameManager.Instance.GameLoop += _followPath.ActivateFollowPath;
        GameManager.Instance.GoblinEnding = true;
        Invoke("FuiteEndSound", 5f); 
    }

    private void FuiteEndSound()
    {
        PlaySound(_audioSource, _fuiteCancelSound);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>())
            GameManager.Instance.PlayerHUDController.LoreText(_appearText);
    }


    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.OBJECTIF:
                    _uiObjectToUse.Despawn();
                    Fuite();
                    break;
            }
            GameManager.Instance.GameLoop -= ActivedObject;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            GameManager.Instance.GameLoop += ActivedObject;
            switch (uiObject.ObjectType)
            {
                case EUIObject.EASYSWORD:
                    TakeDamage(_easySwordAtk, uiObject);
                    GameManager.Instance.PlayerHUDController.LoreText(_easySwordText);
                    break;
                case EUIObject.MEDIUMSWORD:
                    TakeDamage(_mediumSwordAtk, uiObject);
                    GameManager.Instance.PlayerHUDController.LoreText(_mediumSwordText);
                    break;
                case EUIObject.HARDSWORD:
                    TakeDamage(_hardSwordAtk, uiObject);
                    GameManager.Instance.PlayerHUDController.LoreText(_hardSwordText);
                    break;
                case EUIObject.OBJECTIF:
                    InRangeUIObject(uiObject);
                    break;
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            _uiObjectToUse = null;
            GameManager.Instance.GameLoop -= ActivedObject;
            switch (uiObject.ObjectType)
            {
                case EUIObject.OBJECTIF:
                    uiObject.HighlightObject(false);
                    break;
            }
        }

    }


}