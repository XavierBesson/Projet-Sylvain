using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{

   

   [SerializeField] private CharacterController _player;

  [SerializeField]  private float _damage = 1;

    private bool _inRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   

    public void TakeDamage()
    {
        if (_inRange)
        {
            _player.Hpdamage(_damage);

           Invoke("TakeDamage", 1f);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        _inRange = true;
        TakeDamage(); 
    }

    private void OnTriggerExit(Collider other)
    {
        _inRange = false;
      
    }
}
