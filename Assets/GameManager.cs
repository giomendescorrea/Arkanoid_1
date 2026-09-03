using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static int PlayerScore = 0; 
    public static int LastScore = 0; 
    public static int Lives = 3; 
    public static bool isGameStarted = false; 

    public static string lastPlayedScene = "Cena_1"; 

    // Variável para controlar a quantidade de blocos restante na fase
    private int totalBlocos = 0;

    public GUISkin layout;               
    private GameObject theBall;          

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");

        string cenaAtual = SceneManager.GetActiveScene().name;

        if (cenaAtual != "Intro" && cenaAtual != "Win" && cenaAtual != "Lose")
        {
            lastPlayedScene = cenaAtual;
            isGameStarted = true;

            // Conta automaticamente quantos blocos existem no início da fase
            ContarBlocosIniciais();
        }

        if (cenaAtual == "Intro")
        {
            isGameStarted = false; 
            PararBola();
        }

        AtualizarSpritesVidas();
    }

    private void ContarBlocosIniciais()
    {
        GameObject[] blocos = GameObject.FindGameObjectsWithTag("Brick");
        totalBlocos = blocos.Length;
        Debug.Log("Total de blocos no início da fase: " + totalBlocos);
    }

    // Chamado pela bola SEMPRE que um bloco é atingido
    public void BlocoDestruido()
    {
        totalBlocos--; // Subtrai 1 do total
        Debug.Log("Blocos restantes: " + totalBlocos);

        // Se o total chegou a zero, passa de fase imediatamente
        if (totalBlocos <= 0)
        {
            ChecarFimDeFase();
        }
    }

    private void ChecarFimDeFase()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Cena_1" || sceneName == "Cena1")
        {
            CarregarCena("Cena_2", "Cena2");
        } 
        else if (sceneName == "Cena_2" || sceneName == "Cena2")
        {
            CarregarCena("Cena_3", "Cena3");
        }
        else if (sceneName == "Cena_3" || sceneName == "Cena3")
        {
            GanharJogo();
        }
    }

    private void CarregarCena(string opcaoComUnderline, string opcaoSemUnderline)
    {
        isGameStarted = true;
        if (Application.CanStreamedLevelBeLoaded(opcaoComUnderline))
        {
            SceneManager.LoadScene(opcaoComUnderline);
        }
        else if (Application.CanStreamedLevelBeLoaded(opcaoSemUnderline))
        {
            SceneManager.LoadScene(opcaoSemUnderline);
        }
        else
        {
            Debug.LogError("A cena não foi encontrada no Build Settings!");
        }
    }

    public static void Score(string brickID)
    {
        if (brickID == "Brick")
        {
            PlayerScore++;
        }
    }

    public void GanharJogo()
    {
        LastScore = PlayerScore;
        isGameStarted = false;
        SceneManager.LoadScene("Win");
    }

    public void PerderVida()
    {
        Lives--;

        if (Lives == 2 && life3 != null) life3.SetActive(false);
        else if (Lives == 1 && life2 != null) life2.SetActive(false);
        else if (Lives <= 0)
        {
            if (life1 != null) life1.SetActive(false);
            LastScore = PlayerScore;
            isGameStarted = false;
            SceneManager.LoadScene("Lose");
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
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    void OnGUI()
    {
        if (layout != null) GUI.skin = layout;

        string cenaAtual = SceneManager.GetActiveScene().name;

        if (!isGameStarted && cenaAtual == "Intro")
        {
            GUIStyle titleStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            titleStyle.fontSize = 28;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.normal.textColor = Color.white;
            titleStyle.wordWrap = true;

            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 220, 600, 110), 
                "IM REALLY MAD TODAY,\nLETS BREAK SOME BRICKS", titleStyle);

            GUIStyle subTitleStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            subTitleStyle.fontSize = 18;
            subTitleStyle.fontStyle = FontStyle.Italic;
            subTitleStyle.alignment = TextAnchor.MiddleCenter;
            subTitleStyle.normal.textColor = Color.yellow;
            subTitleStyle.wordWrap = true;

            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 90, 500, 70), 
                "help me move my spaceship\nusing left and right arrows", subTitleStyle);

            GUI.skin.button.fontSize = 32;
            if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 + 10, 280, 70), "START GAME"))
            {
                isGameStarted = true;
                Lives = 3;
                PlayerScore = 0;

                CarregarCena("Cena_1", "Cena1");
            }

            GUIStyle scoreStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            scoreStyle.fontSize = 22;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 100, 400, 40), 
                "Última Pontuação: " + LastScore, scoreStyle);
        }
        else if (cenaAtual == "Win" || cenaAtual == "Lose")
        {
            GUIStyle resultStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            resultStyle.fontSize = 36;
            resultStyle.fontStyle = FontStyle.Bold;
            resultStyle.alignment = TextAnchor.MiddleCenter;
            resultStyle.normal.textColor = cenaAtual == "Win" ? Color.green : Color.red;

            string mensagem = cenaAtual == "Win" ? "YOU WIN!" : "GAME OVER";
            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 120, 400, 60), mensagem, resultStyle);

            GUIStyle scoreStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            scoreStyle.fontSize = 22;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 50, 400, 40), 
                "Pontuação Final: " + LastScore, scoreStyle);

            GUI.skin.button.fontSize = 30;
            if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 + 30, 280, 70), "TRY AGAIN"))
            {
                PlayerScore = 0;
                Lives = 3;
                isGameStarted = true;

                CarregarCena(lastPlayedScene, lastPlayedScene); 
            }
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
                isGameStarted = false;
                lastPlayedScene = "Cena_1";
                
                SceneManager.LoadScene("Intro"); 
            }
        }
    }
}