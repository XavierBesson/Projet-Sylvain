using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        GameManager.Instance.PlayerHUDController = this;

        _eventText.text = "extrèmement long test que je vois ce que ça rend";
        Invoke("LoreTextEmpty", 2);
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


    public void ChangeVolume()
    {
        GameManager.Instance.SoundMultiplier = _volumeBar.value;
    }


    public void ChangeHPDisplay(float hp)
    {
        _hpBar.value = hp;
    }

    public void LoreText(string textToShow)
    {
        _eventText.text = textToShow;
        Invoke("LoreTextEmpty", 2);
    }

    public void LoreTextEmpty ()
    {
        _eventText.text = string.Empty;
    }

    public void PlayerIsDead(bool stairs)
    {
        foreach (GameObject gameobject in _deadUiObjects)
        {
            gameobject.SetActive(true);
        }

        if (stairs == true)
        {
            _deathText.text = "Le plus ancien ennemie du monde : La gravité ! (aider par sa cousine la difficulté)";
        }
        else _deathText.text = "Une mort à COUPER le souffle";

        //Afficher text 
        //Couper la visibilité
        //Bouton recommencer et quitter
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


    public void ReloadButton()
    {
        SceneManager.LoadScene("LdScene"); 
    }

    public void QuitBButton()
    {
        Application.Quit();
    }

}
