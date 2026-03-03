using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeBarObject : UIObject
{
    [SerializeField] private LayerMask _doorLayer;
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
            collision.gameObject.GetComponentInParent<Door>().SoundUsed();

            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer == _doorLayer)
            _onDoor = false;
    }




    private void OpenDoor()
    {
        if (_onDoor)
        {
            GameManager.Instance.Door.SoundUsed();
        }
    }
}
