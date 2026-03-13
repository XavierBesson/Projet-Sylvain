using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    [SerializeField] private Animator _chestAnimator;

    [Header("Ending Settings")]
    [SerializeField] private GameObject[] _GoldObjects;

    //[SerializeField] private GameObject _vcEnding;

    private bool _spin = false;
    [Header("Explosion Settings")]
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _radius = 0.5f;
    [SerializeField] private float _force = 10f;
    [SerializeField] private float _delay = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Endings()
    {
        if (GameManager.Instance.GoblinEnding)
        {
            foreach (GameObject gold in _GoldObjects)
            {
                gold.SetActive(false);
            }
            _chestAnimator.SetTrigger("OpenChest");
        }
        else
            GoldEnding();
    }

    public void SpinChest()
    {
        _chestAnimator.SetBool("OIIA", _spin);
    }

    private void GoldEnding()
    {
        _chestAnimator.SetTrigger("OpenChest");

        Invoke("Coinplosion", _delay);
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
