using UnityEngine;

public class AutoPaddle : MonoBehaviour
{
    [Header("Configurações da IA")]
    public float speed = 10f; // Velocidade de acompanhamento do paddle
    public float xBound = 3.22f; // Limite lateral para não sair da tela

    private Transform ballTransform;

    void Update()
    {
        if (ballTransform == null)
        {
            GameObject ball = GameObject.FindGameObjectWithTag("Ball");
            if (ball != null) ballTransform = ball.transform;
            return;
        }

        Vector3 targetPosition = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);
        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -xBound, xBound);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}