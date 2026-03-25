using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DeadBody : EnigmeObject
{
    [SerializeField] private GameObject _skeleton;
    [SerializeField] private GameObject _grave;
    [SerializeField] private string _playSceneToLoad;
    [SerializeField] private float _delayBeforeLoadingScene = 2;


    private void Revive()
    {
        SceneManager.LoadScene(_playSceneToLoad);
    }


    private void Die()
    {
        _skeleton.SetActive(false);
        _grave.SetActive(true);
        Invoke("QuitGame", _delayBeforeLoadingScene);
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }



    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.RETRYGAME:
                    Revive();
                    _uiObjectToUse.Despawn();
                    break;
                case EUIObject.QUITGAME:
                    Die();
                    _uiObjectToUse.Despawn();
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
                case EUIObject.RETRYGAME:
                    _uiObjectToUse = uiObject;
                    InRangeUIObject(uiObject);
                    break;
                case EUIObject.QUITGAME:
                    _uiObjectToUse = uiObject;
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
            uiObject.HighlightObject(false);
        }
    }

}