using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Walls : MonoBehaviour 
{
    public AudioSource source;

    void OnCollisionEnter2D(Collision2D coll) 
    {
        source = GetComponent<AudioSource>();

        if (coll.collider.CompareTag("Ball"))
        {
            GameManager gm = GameManager.instance;
            if (gm == null)
            {
                gm = FindFirstObjectByType<GameManager>();
            }

            if (gm != null)
            {
                gm.PerderVida();

                if (source != null)
                {
                    source.Play();
                }
            }

            if (GameManager.Lives <= 0)
            {
                GameManager.LastScore = GameManager.PlayerScore;
                GameManager.isGameStarted = false;
                
                SceneManager.LoadScene("Lose");
            }
            else
            {
                coll.gameObject.SendMessage("RestartGame", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}