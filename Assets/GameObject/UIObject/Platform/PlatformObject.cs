using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : UIObject
{
    // Start is called before the first frame update
    public override void Update()
    {
        if (IsDragging && Input.GetMouseButtonUp(1))
        {
            Undrag();
           
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
       
   if (collision.gameObject.GetComponentInParent<PiegedFloor>() != null)
        {
            collision.gameObject.GetComponentInParent<PiegedFloor>().IsCovered();

            Destroy(gameObject);
        }
    }





  
}
