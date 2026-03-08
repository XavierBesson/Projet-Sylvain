using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{

    [SerializeField] private GameObject _stopBackwardWalls;
    [SerializeField] private bool _ramp;
    [SerializeField] private bool _active; 


    // Start is called before the first frame update
    void Start()
    {
        _stopBackwardWalls.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {if (_ramp == true)
        {
            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                //Rien
                GameManager.Instance.PlayerHUDController.LoreText("Un peu grand ces marches...");
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                //prend moitié PV
                GameManager.Instance.Player.Hpdamage(1.5f);
                // Empèche le jour de reculer 
                _stopBackwardWalls.gameObject.SetActive(true);
                GameManager.Instance.PlayerHUDController.LoreText("Ouch ! Trop haute ces marches !");
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
                //Prend tout pv
                GameManager.Instance.Player.Hpdamage(GameManager.Instance.Player.Hp);
                // Empèche le jour de reculer 
                _stopBackwardWalls.gameObject.SetActive(true);
                GameManager.Instance.PlayerHUDController.LoreText("Les dégats de chute rigolent pas en DIFFICILE !");
            }
        }
        else _stopBackwardWalls.gameObject.SetActive(_active);
    }




    }
