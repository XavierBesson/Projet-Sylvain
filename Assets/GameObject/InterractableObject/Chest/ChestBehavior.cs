using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    [SerializeField] private Animator _chestAnimator;
    private bool _spin = false;
    [Header("Explosion Settings")]
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _force = 10f;
    [SerializeField] private float _delay = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Coinplosion", _delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spin =_chestAnimator.GetBool("OIIA");

            _chestAnimator.SetBool("OIIA", !_spin);
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            _chestAnimator.SetTrigger("OpenChest");
        }
    }

    private void Coinplosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + _position, _radius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(_force, transform.position + _position, _radius);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _position, _radius);
    }
}
