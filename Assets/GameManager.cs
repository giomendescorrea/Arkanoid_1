using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{
    public static int PlayerScore = 0; 
    public static int LastScore = 0; 
    public static int Lives = 3; 
    public static bool isGameStarted = false; 
    public static string lastPlayedScene = "Cena1"; 

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
            PlayerScore = 0;
            Lives = 3;

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

    public void BlocoDestruido()
    {
        totalBlocos--; 
        Debug.Log("Blocos restantes: " + totalBlocos);

        if (totalBlocos <= 0)
        {
            ChecarFimDeFase();
        }
    }

    private void ChecarFimDeFase()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Cena1")
        {
            CarregarCena("Cena2");
        } 
        else if (sceneName == "Cena2")
        {
            CarregarCena("Cena3");
        }
        else if (sceneName == "Cena3")
        {
            GanharJogo();
        }
    }

    private void CarregarCena(string opcao)
    {
        isGameStarted = true;
        if (Application.CanStreamedLevelBeLoaded(opcao))
        {
            SceneManager.LoadScene(opcao);
        }
        else
        {
            Debug.LogError("A cena " + opcao + " não foi encontrada no Build Settings!");
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

            GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 400, 600, 100), 
                "HI, I'M A VISITOR,\nLETS BREAK SOME BRICKS WITH ME", titleStyle);

            GUIStyle subTitleStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            subTitleStyle.fontSize = 18;
            subTitleStyle.fontStyle = FontStyle.Italic;
            subTitleStyle.alignment = TextAnchor.MiddleCenter;
            subTitleStyle.normal.textColor = Color.yellow;
            subTitleStyle.wordWrap = true;

            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 270, 500, 60), 
                "help me move my spaceship\nusing left and right arrows", subTitleStyle);

            GUI.skin.button.fontSize = 32;
            if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 - 180, 280, 65), "START GAME"))
            {
                isGameStarted = true;
                Lives = 3;
                PlayerScore = 0;

                CarregarCena("Cena1");
            }

            GUIStyle scoreStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            scoreStyle.fontSize = 22;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 80, 400, 40), 
                "Última Pontuação: " + LastScore, scoreStyle);
        }
        else if (cenaAtual == "Win" || cenaAtual == "Lose")
        {
            GUIStyle resultStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            resultStyle.fontSize = 36;
            resultStyle.fontStyle = FontStyle.Bold;
            resultStyle.alignment = TextAnchor.MiddleCenter;
            resultStyle.normal.textColor = cenaAtual == "Win" ? Color.white : Color.red;

            GUIStyle scoreStyle = layout != null ? new GUIStyle(GUI.skin.label) : new GUIStyle();
            scoreStyle.fontSize = 22;
            scoreStyle.alignment = TextAnchor.MiddleCenter;
            scoreStyle.normal.textColor = Color.white;

            GUI.skin.button.fontSize = 30;

            if (cenaAtual == "Lose")
            {
                // Posições ajustadas para a tela de Lose
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 60, 400, 60), "GAME OVER", resultStyle);
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 120, 400, 40), "Pontuação Final: " + LastScore, scoreStyle);

                if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 + 180, 280, 70), "TRY AGAIN"))
                {
                    PlayerScore = 0;
                    Lives = 3;
                    isGameStarted = true;
                    CarregarCena(lastPlayedScene);
                }
            }
            else if (cenaAtual == "Win")
            {
                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 120, 400, 60), "YOU WIN!", resultStyle);

                GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 180, 400, 40), "Pontuação Final: " + LastScore, scoreStyle);

                if (GUI.Button(new Rect(Screen.width / 2 - 140, Screen.height / 2 + 240, 280, 70), "MAIN MENU"))
                {
                    PlayerScore = 0;
                    Lives = 3;
                    isGameStarted = false;
                    lastPlayedScene = "Cena1";
                    SceneManager.LoadScene("Intro");
                }
            }
        }
        else
        {
            GUI.skin.label.fontSize = 30;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.Label(new Rect(20, 20, 200, 50), "Pontos: " + PlayerScore);
        }
    }
}