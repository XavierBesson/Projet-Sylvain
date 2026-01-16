using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachableUi : MonoBehaviour
{
    [SerializeField] private bool _attached = true;

    public bool Attached
    {
        get
        {
            return _attached;
        }
        set
        {
            _attached = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
