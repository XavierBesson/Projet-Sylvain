using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
