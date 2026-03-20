using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmeObject : MonoBehaviour
{
    private protected CharacterController _player;
    private protected UIObject _uiObjectToUse;

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


    public virtual void ActivedObject()
    {

    }



    protected void InRangeUIObject(UIObject uiObject)
    {
        _uiObjectToUse = uiObject;
        uiObject.HighlightObject(true);
    }

}
