using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClicableObject : MonoBehaviour
{
    [SerializeField] private string _clicText = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText()
    {
        GameManager.Instance.PlayerHUDController.LoreText(_clicText);
    }
}
