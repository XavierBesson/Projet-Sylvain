using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : EnigmeObject
{
    [SerializeField] private int _maxHp = 3;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _dieSound;
    [SerializeField] private AudioClip _fuiteSound;

    [SerializeField] private GameObject _deathplacment;
    private int _currentHp;
    [SerializeField] private FollowPath _followPath;

    [SerializeField] private ParticleSystem _easyBlood;
    [SerializeField] private ParticleSystem _mediumBlood;
    [SerializeField] private ParticleSystem _hardBlood;
    [SerializeField] private GameObject _bloodPlacement;

    [SerializeField] private GameObject _body1;
    [SerializeField]  private GameObject _body2;
    [SerializeField] private GameObject _body3;

    [SerializeField] private GameObject _easySword;
    [SerializeField] private GameObject _mediumSword;
    [SerializeField] private GameObject _hardSword;


    void Start()
    {
        _currentHp = _maxHp;
        _body1.gameObject.SetActive(true);
        _body2.gameObject.SetActive(false);
        _body3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        print(_currentHp);
        PlaySound(_audioSource, _damageSound);

        if (_currentHp <= 0)
        {
            PlaySound(_audioSource, _dieSound);
           // Destroy(gameObject);

           //gameObject.transform.position = _deathplacment.transform.position;
        //    gameObject.transform.localScale = _deathplacment.transform.localScale;
          //  gameObject.transform.eulerAngles = _deathplacment.transform.eulerAngles;
            //vérouiller la difficulté ? 
            _body1.gameObject.SetActive(false);
            _body2.gameObject.SetActive(true);
            _body3.gameObject.SetActive(true);

            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                _easySword.gameObject.SetActive(true);
                _mediumSword.gameObject.SetActive(false);
                _hardSword.gameObject.SetActive(false);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                _easySword.gameObject.SetActive(false);
                _mediumSword.gameObject.SetActive(true);
                _hardSword.gameObject.SetActive(false);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
                _easySword.gameObject.SetActive(false);
                _mediumSword.gameObject.SetActive(false);
                _hardSword.gameObject.SetActive(true);
            }

            Destroy(GetComponent<BoxCollider>());


           // GameManager.Instance.PlayerHUDController.LoreText("Une belle décapitation ! Même pas d'éffort !");

        }

     if(GameManager.Instance.Difficulty == EDifficulty.EASY)
        {
            _easyBlood = Instantiate(_easyBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
            _easyBlood.Play();
        }
     else if(GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
        {
            _mediumBlood = Instantiate(_mediumBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
            _mediumBlood.Play();

        }
     else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
        {
            _hardBlood = Instantiate(_hardBlood, _bloodPlacement.gameObject.transform.position, Quaternion.identity);
            _hardBlood.Play();
        }

    }


    public void Fuite()
    {
        PlaySound(_audioSource, _fuiteSound);
        GameManager.Instance.GameLoop += _followPath.ActivateFollowPath;
        GameManager.Instance.GoblinEnding = true;
    }


}
