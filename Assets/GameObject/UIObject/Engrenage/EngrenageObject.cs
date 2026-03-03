using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EngrenageObject : UIObject
{
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private GameObject _door; 
    private bool _onDoor = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Door>() != null)
        {
            collision.gameObject.GetComponentInParent<Door>().Engranage();

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == _doorLayer)
            _onDoor = false;
    }




    public override void Undrag()
    {
        base.Undrag();
        if (_onDoor)
        {
            GameManager.Instance.Door.Engranage();
        }
    }
}
