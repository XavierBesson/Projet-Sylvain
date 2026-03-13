using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private bool _endingNormal = true;
    [SerializeField] private FollowPath _followPath;
    [SerializeField] private EndingCat _endingCat;
    [SerializeField] private ChestBehavior _chest;
    [SerializeField] private GameObject _cat;


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
        if (_endingNormal == true)
        {
            //Trigger event du coffre ? 

            Invoke("EndingScreenDelay", 5);
        }
        if(_endingNormal ==false)
        {
            _followPath.ActivateFollowPath();
            _cat.SetActive(true);
            _endingCat.ActivateCat();

            Invoke("EndingScreenDelay", 5);
        }
     

    }
    public void EndingCat()
    {
        _endingNormal = false;
    }

    public void EndingScreenDelay()
    {
        GameManager.Instance.PlayerHUDController.Ending(_endingNormal);
    }

    }
