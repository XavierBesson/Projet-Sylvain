using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeObject : MonoBehaviour
{
    private protected CharacterController _player;

    void Start()
    {
        _player = GameManager.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    protected void Effect()
    {

    }


    protected void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.volume = GameManager.Instance.SoundMultiplier;
        audioSource.Play();
    }


}
