using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private bool _normalEnding = true;
    [SerializeField] private FollowPath _followPath;
    [SerializeField] private EndingCat _endingCat;
    [SerializeField] private ChestBehavior _chest;
    [SerializeField] private GameObject _cat;

    [Header("NormalEnding")]
    [SerializeField] private string _normalEndingText;
    [SerializeField] private AudioClip _normalEndingMusic;

    [Header("CatEnding")]
    [SerializeField] private string _catEndingText;
    [SerializeField] private AudioClip _catEndingMusic;



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
    }


    private void NormalEnding()
    {
        _chest.Endings();
        GameManager.Instance.PlayerHUDController.LoreText(_normalEndingText);
        GameManager.Instance.PlayGameManagerSouds(_normalEndingMusic);
    }


    private void CatEnding()
    {
        GameManager.Instance.PlayerHUDController.LoreText(_catEndingText);
        GameManager.Instance.PlayGameManagerSouds(_catEndingMusic);
        _chest.SpinChest();
        _followPath.ActivateFollowPath();
        _cat.SetActive(true);
        _endingCat.ActivateCat();
    }



    public void EndingCat()
    {
        _normalEnding = false;
        
    }

    public void EndingScreenDelay()
    {
        GameManager.Instance.PlayerHUDController.Ending(_normalEnding);
    }

    }
