using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordObject : UIObject
{

    [SerializeField] private int _power = 1;
    [SerializeField] private float _atkSpeed = 1;
    [SerializeField] private string _hitTextEasy = string.Empty;
    [SerializeField] private string _hitTextMedium = string.Empty;
    [SerializeField] private string _hitTextHard = string.Empty;

    private void OnTriggerEnter(Collider collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            print(_rb.velocity.magnitude);
            if (_rb.velocity.magnitude >= _atkSpeed)
            {
                if (GameManager.Instance.Difficulty == EDifficulty.EASY)
                {
                    GameManager.Instance.PlayerHUDController.LoreText(_hitTextEasy);
                }
                else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
                {
                    GameManager.Instance.PlayerHUDController.LoreText(_hitTextMedium);
                }
               else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
                {
                    GameManager.Instance.PlayerHUDController.LoreText(_hitTextHard);
                }


                enemy.TakeDamage(_power,gameObject);




            }
        }
    }

}
