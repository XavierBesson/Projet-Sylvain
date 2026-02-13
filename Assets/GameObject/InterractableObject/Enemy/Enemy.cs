using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnigmeObject
{
    [SerializeField] private int _maxHp = 3;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _dieSound;
    private int _currentHp;

    
    void Start()
    {
        _currentHp = _maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
        print(_currentHp);
        PlaySound(_audioSource, _damageSound);

        if (_currentHp <= 0)
        {
            PlaySound(_audioSource, _dieSound);
            Destroy(gameObject);
        }
        
    }


}
