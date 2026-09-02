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
        // Busca a instância do GameManager com garantia, evitando erro de nulo
            GameManager gm = GameManager.instance;
            if (gm == null)
            {
                gm = FindFirstObjectByType<GameManager>();
            }

            if (gm != null)
            {
                gm.PerderVida();
                if (source != null){
                    source.Play();
                }
            }

            // Verifica se as vidas zeraram após a perda da vida
            if (GameManager.Lives <= 0)
            {
                // Game Over: Salva a pontuação e recarrega a cena
                GameManager.LastScore = GameManager.PlayerScore;
                GameManager.isGameStarted = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                // Reposiciona a bola sem resetar a cena inteira
                coll.gameObject.SendMessage("RestartGame", null, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}