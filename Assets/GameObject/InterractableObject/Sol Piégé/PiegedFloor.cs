using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{

   

  [SerializeField] private CharacterController _player;
  [SerializeField] private float _damage = 1;
  [SerializeField] private GameObject _platform = null;
 [SerializeField] private bool _isCovered;

  private bool _inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        _platform.SetActive(false);
        Invoke("IsCovered", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   

    public void TakeDamage()
    {
        if (_inRange && _isCovered == false)
        {
            _player.Hpdamage(_damage);

          // Invoke("TakeDamage", 1f);
            
        }
    }

    public void IsCovered()
    {
        _isCovered = true;
        _platform.SetActive(true);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage();
        _inRange = true;
       
    }

    private void OnTriggerExit(Collider other)
    {
        _inRange = false;
      
    }


}
