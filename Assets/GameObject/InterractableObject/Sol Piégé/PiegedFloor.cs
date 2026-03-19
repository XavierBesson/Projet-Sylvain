using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{
    [SerializeField] private float _damage = 1;
    [SerializeField] private GameObject _platformHp = null;
    [SerializeField] private GameObject _platformBackground = null;
    [SerializeField] private Door _door; 

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
            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                _player.Hpdamage(_damage / 3);
                _door.SpikeDamage(_damage/3);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                _player.Hpdamage(_damage);
                _door.SpikeDamage(_damage);
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
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



    private void OnCollisionStay(Collision collision)
    {
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            if (Input.GetMouseButtonUp(1))
            {
                switch (uiObject.ObjectType)
                {
                    case EUIObject.PLATFORM:
                        IsCovered(false);
                        uiObject.Despawn();
                        break;
                    case EUIObject.HEALTHBAR:
                        IsCovered(true);
                        uiObject.Despawn();
                        break;
                }
            }
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<CharacterController>())
        {
            _player.IsInStairs(false);

            _inRange = true;
            TakeDamage();

            Debug.Log("marche pique");
        }
        UIObject uiObject = collision.gameObject.GetComponent<UIObject>();
        if (uiObject != null)
        {
            switch (uiObject.ObjectType)
            {
                case EUIObject.PLATFORM:
                    uiObject.DoNotDespawn = true;
                    print(uiObject.DoNotDespawn);
                    uiObject.HighlightObject(true);
                    break;
                case EUIObject.HEALTHBAR:
                    uiObject.DoNotDespawn = true;
                    uiObject.HighlightObject(true);
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
