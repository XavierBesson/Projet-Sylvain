using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class MainMenuDoor : EnigmeObject
{
    [SerializeField] private MainMenuButtons _mainMenu;
    private MainMenuKey _keyInSerrure;
    [SerializeField] private MeshRenderer _clefModel;
    [SerializeField] private Material _playMat;
    [SerializeField] private Material _tutoMat;
    [SerializeField] private Material _quitMat;
    [SerializeField] private Material _returnMat;

    [Header("Ejection settings")]
    [SerializeField] private float _delayBeforeLoadingScene = 0.5f;
    [SerializeField] private Transform _ejectionTransform;
    [SerializeField] private Vector3 _minRange;
    [SerializeField] private Vector3 _maxRange;


    void Start()
    {
        _clefModel.enabled = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonUp(1) && _keyInSerrure != null)
        {
            EjectPreviousKey();
            OpenDoor();
        }
    }

    



    private void OnTriggerEnter(Collider other)
    {
        _keyInSerrure = other.gameObject.GetComponentInParent<MainMenuKey>();
    }

    private void OnTriggerExit(Collider other)
    {
        _keyInSerrure = null;
    }


    private void OpenDoor()
    {
        switch (_keyInSerrure.Key)
        {
            case EMainMenuKey.PLAY:
                DisplayKey(_playMat);
                Invoke("PlayGame", _delayBeforeLoadingScene);
                break;
            case EMainMenuKey.TUTO:
                DisplayKey(_tutoMat);
                _mainMenu.DisplayTuto();
                break;
            case EMainMenuKey.QUIT:
                DisplayKey(_quitMat);
                Invoke("QuitGame", _delayBeforeLoadingScene);
                break;
            case EMainMenuKey.MAINMENU:
                DisplayKey(_returnMat);
                _mainMenu.DisplayMainMenu();
                break;
        }
        _keyInSerrure.ReturnToUI();
        _keyInSerrure = null;
    }

    private void DisplayKey(Material mat)
    {
        _clefModel.enabled = true;
        _clefModel.material = mat;
    }

    private void EjectPreviousKey()
    {
        GameObject key = Instantiate(_clefModel.gameObject, _ejectionTransform.position, _ejectionTransform.rotation, transform);
        Rigidbody rb = key.AddComponent<Rigidbody>();
        BoxCollider collider = key.AddComponent<BoxCollider>();

        Vector3 propulsion = new Vector3(Random.Range(_minRange.x, _maxRange.x),
            Random.Range(_minRange.y, _maxRange.y), Random.Range(_minRange.z, _maxRange.z));

        rb.AddForce(propulsion,ForceMode.VelocityChange);
    }

    private void PlayGame()
    {
        _mainMenu.PlayGame();
    }

    private void QuitGame()
    {
        _mainMenu.QuitGame();
    }
}