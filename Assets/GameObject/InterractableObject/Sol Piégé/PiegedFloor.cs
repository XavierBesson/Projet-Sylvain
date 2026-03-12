using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiegedFloor : EnigmeObject
{

   

  [SerializeField] private CharacterController _player;
  [SerializeField] private float _damage = 1;
  [SerializeField] private GameObject _platform = null;
 [SerializeField] private bool _isCovered;
    [SerializeField] private Door _door; 

  private bool _inRange = false;

    // Start is called before the first frame update
    void Start()
    {
        _platform.SetActive(false);
       // Invoke("IsCovered", 5f);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


   

    public void TakeDamage()
    {
        if (_inRange ==true && _isCovered == false)
        {

            _player.Hpdamage(_damage);

            _door.SpikeDamage(_damage); 
          

           Invoke("TakeDamage", 0.1f);
            
        }
    }

    public void IsCovered()
    {
        _isCovered = true;
        _platform.SetActive(true);

        _platform.GetComponent<MeshRenderer>().material.color = Color.blue;


    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponentInParent<CharacterController>() == _player )
        {
            _inRange = true;
            TakeDamage();

            Debug.Log("marche pique");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _inRange = false;
        Debug.Log("sort pique");

    }


}
