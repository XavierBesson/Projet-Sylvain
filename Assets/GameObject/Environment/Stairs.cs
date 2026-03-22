using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private bool _stairUsed = false;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _FallingDamageSound;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.GetComponent<CharacterController>() && !_stairUsed)
        {
            if (GameManager.Instance.Difficulty == EDifficulty.EASY)
            {
                //Rien
                GameManager.Instance.PlayerHUDController.LoreText("Damn, these stairs ARE in fact high. Careful mate.");
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.MEDIUM)
            {
                //prend moitié PV
                GameManager.Instance.Player.Hpdamage(1.5f, true);
                GameManager.Instance.PlayerHUDController.LoreText("Ouch ! That must have hurt ! Are you okay ?");
            }
            else if (GameManager.Instance.Difficulty == EDifficulty.HARD)
            {
                //Prend tout pv
                GameManager.Instance.Player.Hpdamage(GameManager.Instance.Player.Hp, true);
                GameManager.Instance.PlayerHUDController.LoreText("Did you put the game in Difficult ? Too bad for you !");
            }
            _stairUsed = true;
        }
    }
}
