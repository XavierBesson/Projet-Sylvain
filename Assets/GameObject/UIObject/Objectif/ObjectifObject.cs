using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectifObject : UIObject
{
    private Enemy _enemy = null;

    private void OnTriggerEnter(Collider collision)
    {
        _enemy = collision.gameObject.GetComponent<Enemy>();
    }

    private void OnTriggerExit(Collider other)
    {
        _enemy = null;
    }




    public override void OnMouseUp()
    {
        base.OnMouseUp();
        if (_enemy != null)
        {
            _enemy.Fuite();
        }
    }
}
