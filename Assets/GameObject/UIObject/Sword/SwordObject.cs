using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordObject : UIObject
{

    [SerializeField] private int _power = 1;
    [SerializeField] private float _atkSpeed = 1;



    void Start()
    {
        
    }

    
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            print(_rb.velocity.magnitude);
            if (_rb.velocity.magnitude >= _atkSpeed)
            {
                enemy.TakeDamage(_power);
            }
        }
    }

}
