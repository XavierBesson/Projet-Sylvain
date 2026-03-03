using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarObject : UIObject
{
    [SerializeField] private LayerMask _doorLayer;
    private bool _onDoor = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<Door>() != null)
        {
            collision.gameObject.GetComponentInParent<Door>().HpBarUsed();

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == _doorLayer)
            _onDoor = false;
    }




    public override void RightMouseUp()
    {
        base.RightMouseUp();
        if (_onDoor)
        {
            GameManager.Instance.Door.HpBarUsed();
        }
    }
}
