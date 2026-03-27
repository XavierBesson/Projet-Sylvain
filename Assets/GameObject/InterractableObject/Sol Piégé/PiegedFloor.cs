using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{
    [SerializeField] private float _damage = 1;
    [SerializeField] private GameObject _platformHp = null;
    [SerializeField] private GameObject _platformBackground = null;
    [SerializeField] private Door _door;
    [SerializeField] private AudioSource _sawSource;

    private bool _inRange = false;
    private bool _takeDamega = true;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameManager.Instance.Player;
        _platformHp.SetActive(false);
        _platformBackground.SetActive(false);
        // Invoke("IsCovered", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        _sawSource.volume = GameManager.Instance.SoundMultiplier;
    }


   

    public void TakeDamage()
    {
        if (_inRange && _takeDamega)
        {
            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                _player.Hpdamage(_damage / 3);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                _player.Hpdamage(_damage);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
                _player.Hpdamage(_damage * 2);
            }
            Invoke("TakeDamage", 0.1f);
        }
    }

    private void Covered(EUIObject uiObject)
    {
        switch (uiObject)
        {
            case EUIObject.PLATFORM:
                _platformBackground.SetActive(true);
                _takeDamega = false;
                break;
            case EUIObject.HEALTHBAR:
                _platformHp.SetActive(true);
                break;
        }  
    }


    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.PLATFORM:
                    Covered(_uiObjectToUse.ObjectType);
                    _uiObjectToUse.Despawn();
                    break;
                case EUIObject.HEALTHBAR:
                    Covered(_uiObjectToUse.ObjectType);
                    _uiObjectToUse.Despawn();
                    break;
            }
            GameManager.Instance.GameLoop -= ActivedObject;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<CharacterController>())
        {
            _player.IsInStairs(false);
            _inRange = true;
            TakeDamage();
        }

        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            if (uiObject.IsDragging)
            {
                GameManager.Instance.GameLoop += ActivedObject;
                switch (uiObject.ObjectType)
                {
                    case EUIObject.PLATFORM:
                        InRangeUIObject(uiObject);
                        uiObject.DoNotDespawn = true;
                        break;
                    case EUIObject.HEALTHBAR:
                        InRangeUIObject(uiObject);
                        uiObject.DoNotDespawn = true;
                        break;
                }
            }
            else
            {
                switch (uiObject.ObjectType)
                {
                    case EUIObject.PLATFORM:
                        Covered(uiObject.ObjectType);
                        uiObject.DoNotDespawn = true;
                        uiObject.Despawn();
                        break;
                    case EUIObject.HEALTHBAR:
                        Covered(uiObject.ObjectType);
                        uiObject.DoNotDespawn = true;
                        uiObject.Despawn();
                        break;
                }
            }
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<CharacterController>())
        {
            _inRange = false;
            Debug.Log("sort pique");
        }
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            _uiObjectToUse = null;
            GameManager.Instance.GameLoop -= ActivedObject;
            switch (uiObject.ObjectType)
            {
                case EUIObject.PLATFORM:
                    uiObject.DoNotDespawn = false;
                    uiObject.HighlightObject(false);
                    break;
                case EUIObject.HEALTHBAR:
                    uiObject.DoNotDespawn = false;
                    uiObject.HighlightObject(false);
                    break;
            }
        }
    }
}
