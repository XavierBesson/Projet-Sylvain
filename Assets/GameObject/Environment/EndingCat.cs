using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCat : MonoBehaviour
{
        [Header("Rotation")]
      [SerializeField]  public float rotationSpeed = 90f;
        private bool _catStart = false;
    
    [Header("Bobbing")]
    [SerializeField] public float _bobHeight = 0.5f;
    [SerializeField] public float _bobSpeed = 2f;

    

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
        if (_catStart)
        {
            // Rotation sur l'axe Y
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

            // Bobbing haut/bas
            float newY = startPosition.y + Mathf.Sin(Time.time * _bobSpeed) * _bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
         }
        }

    public void ActivateCat()
    {
        Invoke("MoveIt", 2);
    }
    public void MoveIt()
    {
        _catStart = true; 
    }
}
