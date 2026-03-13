using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    [SerializeField] private Animator _chestAnimator;
    private bool _spin = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spin =_chestAnimator.GetBool("OIIA");

            _chestAnimator.SetBool("OIIA", !_spin);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            _chestAnimator.SetTrigger("OpenChest");
        }
    }


}
