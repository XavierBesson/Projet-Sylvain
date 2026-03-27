using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PlayerHUDController : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private Slider _volumeBar;
    [SerializeField] private TMP_Dropdown _difficultyButton;
    [SerializeField] private DetachableUi _swordUI;
    [SerializeField] private GameObject[] _swordObjectList;
    [SerializeField] private TextMeshProUGUI _eventText;
    [SerializeField] private GameObject[] _deadUiObjects;
    [SerializeField] private TextMeshProUGUI _deathText;
    [SerializeField] private GameObject _lowHPVisuelImage;
    [SerializeField] private GameObject _damageImage;
    [SerializeField] private GameObject _endingImage;
    [SerializeField] private GameObject _endingImageTroll;
    
    [SerializeField] private float _textTime = 3;
    [SerializeField] private float _actualTextTime = 0;
    [SerializeField] private float _textSpeed = 0.01f;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _textSound;

    private float _aditionnalTextTime = 0;
    private bool _textIsDisplay = false;
    private Coroutine _textShowingCoroutine;



    void Start()
    {
        LoreText("Here you are ! Finally in the dungeon, where a treasure lies. Go get it, now !");
        GameManager.Instance.SoundMultiplier = _volumeBar.value;
    }

    void Update()
    {
        
    }

    public void OpenOptionAnim()
    {
        _animator.SetBool("OptionIsOpen", !_animator.GetBool("OptionIsOpen"));
    }


    public void ChangeDifficulty()
    {
        switch (_difficultyButton.value)
        {
            case 0:
                GameManager.Instance.Difficulty = EDifficulty.HARD;
                _swordUI.UiObject = _swordObjectList[0];
                break;

            case 1:
                GameManager.Instance.Difficulty = EDifficulty.MEDIUM;
                _swordUI.UiObject = _swordObjectList[1];
                break;

            case 2:
                GameManager.Instance.Difficulty = EDifficulty.EASY;
                _swordUI.UiObject = _swordObjectList[2];
                break;
        }
        
        
    }


    #region Text

    public void LoreText(string textToShow)
    {
        if (_textIsDisplay)
        {
            StopCoroutine(_textShowingCoroutine);
            _textShowingCoroutine = null;
        }
        _textShowingCoroutine = StartCoroutine(CharDisplay(textToShow));
        _textIsDisplay = true;
        _actualTextTime = 0;
        _aditionnalTextTime = _textSpeed * textToShow.Length;
        GameManager.Instance.GameLoop -= TextTimer;
        GameManager.Instance.GameLoop += TextTimer;
    }


    IEnumerator CharDisplay(string text)
    {
        char[] chars = text.ToCharArray();
        _eventText.text = string.Empty;

        //audioSource[0].Play();
        //await Awaitable.WaitForSecondsAsync(audioSource[0].clip.length);
        /*
        if (SceneManager.GetActiveScene().buildIndex != 1)
            return;*/

        for (int i = 0; i < chars.Length; i++)
        {
            //audioSource[1].Play();
            _eventText.text += chars[i];
            GameManager.PlaySounds(_audioSource, _textSound);
            yield return new WaitForSeconds(_textSpeed);
            
        }
    }


    private void TextTimer()
    {
        _actualTextTime += Time.deltaTime;
        if (_actualTextTime >= _textTime + _aditionnalTextTime)
        {
            _actualTextTime = 0;
            _eventText.text = string.Empty;
            _textIsDisplay = false;
        }
    }

    #endregion Text


    public void ChangeVolume()
    {
        GameManager.Instance.SoundMultiplier = _volumeBar.value;
    }


    #region HP

    public void ChangeHPDisplay(float hp)
    {
        _hpBar.value = hp;
    }


    public void TakeDammageStart()
    {
        _damageImage.gameObject.SetActive(true);
        Invoke("TakeDamageEnd",0.2f);
    }

    public void TakedamageEnd()
    {
        _damageImage.gameObject.SetActive(false);
    }

    public void LowHpVisuelFeedback(bool isLow)
    {
       if(isLow == true)
        {
        _lowHPVisuelImage.gameObject.SetActive(true);
        }
        else
        {
            _lowHPVisuelImage.gameObject.SetActive(false);
        }
    }

    #endregion HP


    public void ReloadButton()
    {
        SceneManager.LoadScene("LdScene"); 
    }

    public void QuitButton()
    {
        UnityEngine.Application.Quit();
    }

    public void Ending(bool normal)
    {
        if(normal == true)
        {
            _endingImage.SetActive(true);
        }
        else
        {
          _endingImageTroll.SetActive(true);
        }
    }


}
