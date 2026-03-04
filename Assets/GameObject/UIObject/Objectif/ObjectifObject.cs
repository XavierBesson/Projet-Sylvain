using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifObject : UIObject
{
    private Enemy _enemy = null;


    public override void Update()
    {
        if (IsDragging && Input.GetMouseButtonUp(1))
        {
            Undrag();
            Fuite();
        }
    }



    private void OnTriggerEnter(Collider collision)
    {
        _enemy = collision.gameObject.GetComponent<Enemy>();
    }

    private void OnTriggerExit(Collider other)
    {
        _enemy = null;
    }


    private void Fuite()
    {
        if (_enemy != null)
        {
            _enemy.Fuite();
            Despawn();
        }
    }
}
