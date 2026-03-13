using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private bool _endingNormal = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Trigger event du coffre ? 
        Debug.Log("ending");
        GameManager.Instance.PlayerHUDController.Ending(_endingNormal);
    }

    }
