using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenuSkel : MonoBehaviour
{
    [SerializeField] private DeathMenuButtons _mainMenu;
    [SerializeField] private float _delayBeforeLoadingScene = 1f;
    [SerializeField] private GameObject _skeleton;
    [SerializeField] private GameObject _grave;
    [SerializeField] private DeathMenuObject _usedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1) && _usedObject != null)
        {
            PlayDeathOptions();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _usedObject = other.gameObject.GetComponent<DeathMenuObject>();
    }

    private void OnTriggerExit(Collider other)
    {
        _usedObject = null;
    }

    private void PlayDeathOptions()
    {
        switch (_usedObject.Type)
        {
            case EDeathObjectType.RETRY:
                Invoke("Retry", _delayBeforeLoadingScene);
                break;
            case EDeathObjectType.QUIT:
                SpawnGrave();
                Invoke("Quit", _delayBeforeLoadingScene);
                break;
        }
        _usedObject.ReturnToUI();
        _usedObject = null;
    }

    private void Retry()
    {
        _mainMenu.PlayGame();
    }

    private void Quit()
    {
        _mainMenu.QuitGame();
    }

    private void SpawnGrave()
    {
        _skeleton.SetActive(false);
        _grave.SetActive(true);
    }
}
