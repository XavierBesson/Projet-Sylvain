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

    void Start()
    {
        GameManager.Instance.PlayerHUDController = this;
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
                break;

            case 1:
                GameManager.Instance.Difficulty = EDifficulty.MEDIUM;
                break;

            case 2:
                GameManager.Instance.Difficulty = EDifficulty.HARD;
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
}
