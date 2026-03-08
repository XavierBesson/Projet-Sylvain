using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
                GameManager.Instance.Difficulty = EDifficulty.EASY;
                _swordUI.UiObject = _swordObjectList[0];
                break;

            case 1:
                GameManager.Instance.Difficulty = EDifficulty.MEDIUM;
                _swordUI.UiObject = _swordObjectList[1];
                break;

            case 2:
                GameManager.Instance.Difficulty = EDifficulty.HARD;
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

}
