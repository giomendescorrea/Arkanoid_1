using UnityEngine;

public class AutoPaddle : MonoBehaviour
{
    [Header("Configurações da IA")]
    public float speed = 10f; // Velocidade de acompanhamento do paddle
    public float xBound = 3.22f; // Limite lateral para não sair da tela

    private Transform ballTransform;

    void Update()
    {
        // Encontra a bola se ainda não tiver a referência
        if (ballTransform == null)
        {
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if (ball != null) ballTransform = ball.transform;
            return;
        }

        // Define a posição alvo acompanhando apenas o X da bola
        Vector3 targetPosition = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);
        
        // Move a nave suavemente até a posição alvo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Limita o movimento dentro das paredes
        float clampedX = Mathf.Clamp(transform.position.x, -xBound, xBound);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}