using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{
    [SerializeField] private float _damage = 1;
    [SerializeField] private GameObject _platformHp = null;
    [SerializeField] private GameObject _platformBackground = null;
    [SerializeField] private Door _door;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _damageSound;

    private bool _inRange = false;

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
        
    }


   

    public void TakeDamage()
    {
        if (_inRange)
        {
            GameManager.PlaySouds(_audioSource, _damageSound);
            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                GameManager.PlaySouds(_audioSource, _damageSound);
                _player.Hpdamage(_damage / 3);
                _door.SpikeDamage(_damage/3);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                GameManager.PlaySouds(_audioSource, _damageSound);
                _player.Hpdamage(_damage);
                _door.SpikeDamage(_damage);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
                GameManager.PlaySouds(_audioSource, _damageSound);
                _player.Hpdamage(_damage * 2);
                _door.SpikeDamage(_damage * 2);
            }
            Invoke("TakeDamage", 0.1f);
        }
    }

    public void IsCovered(bool hpbar)
    {
        if ((hpbar))
            _platformHp.SetActive(true);
        else
            _platformBackground.SetActive(true);
    }


    public override void ActivedObject()
    {
        base.ActivedObject();
        if (_uiObjectToUse != null && Input.GetMouseButtonUp(1))
        {
            switch (_uiObjectToUse.ObjectType)
            {
                case EUIObject.PLATFORM:
                    IsCovered(false);
                    _uiObjectToUse.Despawn();
                    break;
                case EUIObject.HEALTHBAR:
                    IsCovered(true);
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
