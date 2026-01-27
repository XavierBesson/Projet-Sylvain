using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetachableUi : MonoBehaviour
{
    [SerializeField] private bool _attached = true;
    private Vector2 _initialPosition = Vector2.zero;

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

    public Vector2 InitialPosition
    {
        get { return _initialPosition; }
        set { _initialPosition = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = transform.position;
    }

    public void SetToInitialPosition()
    {
        transform.position = InitialPosition;
    }
}
