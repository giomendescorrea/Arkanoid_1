using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static int PlayerScore = 0; 
    public static int LastScore = 0; 
    public static int Lives = 3; 
    public static bool isGameStarted = false; 

    public GUISkin layout;              
    private GameObject theBall;         

    // Arraste os GameObjects das vidas pelo Inspector no Unity
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    // Instância estática para permitir chamada direta pelo Walls script
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
        isGameStarted = false;
        
        // Mantém os sprites atualizados de acordo com as vidas no início
        AtualizarSpritesVidas();
    }

    public static void Score(string brickID)
    {
        if (brickID == "Brick")
        {
            PlayerScore++;
        }
    }

    // Chamado para ocultar/destruir o sprite da vida perdida
    public void PerderVida()
    {
        Lives--;

        if (Lives == 2 && life3 != null)
        {
            life3.SetActive(false); // ou Destroy(life3);
        }
        else if (Lives == 1 && life2 != null)
        {
            life2.SetActive(false); // ou Destroy(life2);
        }
        else if (Lives <= 0 && life1 != null)
        {
            life1.SetActive(false); // ou Destroy(life1);
        }
    }

    private void AtualizarSpritesVidas()
    {
        if (life1 != null) life1.SetActive(Lives >= 1);
        if (life2 != null) life2.SetActive(Lives >= 2);
        if (life3 != null) life3.SetActive(Lives >= 3);
    }

    void OnGUI()
    {
        if (layout != null)
        {
            GUI.skin = layout;
        }

        if (!isGameStarted)
        {
            GUI.skin.button.fontSize = 45;

            if (GUI.Button(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 40, 320, 80), "START GAME"))
            {
                isGameStarted = true;

                if (Lives <= 0)
                {
                    Lives = 3;
                    PlayerScore = 0;
                    AtualizarSpritesVidas();
                }

                if (theBall != null)
                {
                    theBall.SendMessage("RestartGame", null, SendMessageOptions.DontRequireReceiver);
                }
            }

            GUI.skin.label.fontSize = 25;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 50, 400, 50), "Última Pontuação: " + LastScore);
        }
        else
        {
            GUI.skin.label.fontSize = 30;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(20, 20, 200, 50), "Pontos: " + PlayerScore);

            GUI.skin.button.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width / 2 - 80, 15, 160, 50), "RESTART"))
            {
                LastScore = PlayerScore;
                PlayerScore = 0;
                Lives = 3;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}