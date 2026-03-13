using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : UIObject
{
    private PiegedFloor _solPieged;


    public override void Update()
    {
        if (Input.GetMouseButtonUp(1) && IsDragging)
        {
            Undrag();
            RecouvreSolPiege();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<PiegedFloor>() != null)
        {
            _solPieged = collision.gameObject.GetComponentInParent<PiegedFloor>();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponentInParent<PiegedFloor>() != null)
        {
            _solPieged = null;
        }
    }


    private void RecouvreSolPiege()
    {
        if (_solPieged != null)
        {
            _solPieged.IsCovered(false);
            Despawn();
        }
    }





}
