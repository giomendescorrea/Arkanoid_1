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

        // Nome da sua cena de Intro (Ajuste o texto "Intro" caso sua cena tenha outro nome no Unity)
        string cenaAtual = SceneManager.GetActiveScene().name;

        if (cenaAtual == "Intro")
        {
            // Força a variável a voltar para false ao carregar a tela de Intro
            isGameStarted = false; 
            PararBola();
        }
        else if (cenaAtual == "Cena1")
        {
            // Se já abriu direto na Cena1, marca como iniciado
            isGameStarted = true; 
        }

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
            life3.SetActive(false);
        }
        else if (Lives == 1 && life2 != null)
        {
            life2.SetActive(false);
        }
        else if (Lives <= 0 && life1 != null)
        {
            life1.SetActive(false);
        }
    }

    private void AtualizarSpritesVidas()
    {
        if (life1 != null) life1.SetActive(Lives >= 1);
        if (life2 != null) life2.SetActive(Lives >= 2);
        if (life3 != null) life3.SetActive(Lives >= 3);
    }

    private void PararBola()
    {
        if (theBall != null)
        {
            Rigidbody2D rb = theBall.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero; // Trava a movimentação da bola
            }
        }
    }

    void OnGUI()
    {
        if (layout != null)
        {
            GUI.skin = layout;
        }

        // Exibe a tela de Intro se isGameStarted for false
        if (!isGameStarted)
        {
            // --- TÍTULO PRINCIPAL ---
            GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
            if (layout == null) titleStyle = new GUIStyle();

            titleStyle.fontSize = 28;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = Color.white;
            titleStyle.wordWrap = true;

            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 220, 600, 110), 
                "IM REALLY MAD TODAY,\nLETS BREAK SOME BRICKS", titleStyle);

            // --- INSTRUÇÕES ---
            GUIStyle subTitleStyle = new GUIStyle(GUI.skin.label);
            if (layout == null) subTitleStyle = new GUIStyle();

            subTitleStyle.fontSize = 18;
            subTitleStyle.fontStyle = FontStyle.Italic;
            subTitleStyle.alignment = TextAnchor.MiddleCenter;
            subTitleStyle.normal.textColor = Color.yellow;
            subTitleStyle.wordWrap = true;

            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 90, 500, 70), 
                "help me move my spaceship\nusing left and right arrows", subTitleStyle);

            // --- BOTÃO DE START ---
            GUI.skin.button.fontSize = 32;
            if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 + 10, 280, 70), "START GAME"))
            {
                isGameStarted = true;

                if (Lives <= 0)
                {
                    Lives = 3;
                    PlayerScore = 0;
                }

                // Carrega a Cena1 ao clicar em Start
                SceneManager.LoadScene("Cena1");
            }

            // --- EXIBIÇÃO DE ÚLTIMA PONTUAÇÃO ---
            GUIStyle scoreStyle = new GUIStyle(GUI.skin.label);
            if (layout == null) scoreStyle = new GUIStyle();

            scoreStyle.fontSize = 22;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 100, 400, 40), 
                "Última Pontuação: " + LastScore, scoreStyle);
        }
        else
        {
            // HUD durante a partida na Cena1
            GUI.skin.label.fontSize = 30;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(20, 20, 200, 50), "Pontos: " + PlayerScore);

            GUI.skin.button.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width / 2 - 80, 15, 160, 50), "RESTART"))
            {
                LastScore = PlayerScore;
                PlayerScore = 0;
                Lives = 3;
                isGameStarted = false; // RESETA a flag para ao reiniciar voltar pra Intro
                
                // Redireciona de volta para a cena de Intro
                SceneManager.LoadScene("Intro"); 
            }
        }
    }
}