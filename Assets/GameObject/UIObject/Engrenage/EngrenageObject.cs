using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EngrenageObject : UIObject
{
    [SerializeField] private LayerMask _doorLayer;
    [SerializeField] private GameObject _door; 
    private bool _onDoor = false;


    public override void Update()
    {
        if (IsDragging && Input.GetMouseButtonUp(1))
        {
            Undrag();
            OpenDoor();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Door>() != null)
        {
            _onDoor = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Door>() != null)
            _onDoor = false;
    }




    private void OpenDoor()
    {
        if (_onDoor)
        {
            GameManager.Instance.Door.Engrenage();

            Despawn();
        }
    }
}
