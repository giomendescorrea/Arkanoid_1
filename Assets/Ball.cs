using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public AudioSource source;

    [Header("Configurações de Movimento")]
    public float speed = 10f; 
    public float minVelocityThreshold = 0.5f; 
    public float nudgeForce = 2f; 

    private bool hasBounced = false; 

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        
        if (GameManager.isGameStarted)
        {
            Invoke("GoBall", 0.5f);
        }
    }

    void FixedUpdate()
    {
        if (rb2d.linearVelocity == Vector2.zero) return;

        Vector2 currentVelocity = rb2d.linearVelocity;

        if (hasBounced)
        {
            if (Mathf.Abs(currentVelocity.x) < minVelocityThreshold)
            {
                float dirX = (currentVelocity.x >= 0) ? 1f : -1f;
                currentVelocity.x = dirX * nudgeForce;
            }

            if (Mathf.Abs(currentVelocity.y) < minVelocityThreshold)
            {
                float dirY = (currentVelocity.y >= 0) ? 1f : -1f;
                currentVelocity.y = dirY * nudgeForce;
            }
        }

        rb2d.linearVelocity = currentVelocity.normalized * speed;
    }

    void GoBall()
    {
        hasBounced = false; 

        Vector2 initialDir = new Vector2(0, -2);
        rb2d.linearVelocity = initialDir * speed;
    }

    public void RestartGame()
    {
        CancelInvoke("GoBall");

        rb2d.linearVelocity = Vector2.zero;
        transform.position = new Vector2(0, -2);

        Invoke("GoBall", 0.5f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        hasBounced = true;

        if (coll.collider.CompareTag("Player"))
        {
            float hitPoint = (transform.position.x - coll.transform.position.x) / coll.collider.bounds.size.x;
            Vector2 dir = new Vector2(hitPoint, 1).normalized;
            
            rb2d.linearVelocity = dir * speed;
        }
        else if (coll.gameObject.CompareTag("Brick"))
        {
            if (source != null)
            {
                source.Play();
            }
            
            GameManager.Score("Brick");
            Destroy(coll.gameObject);

            if (GameManager.instance != null)
            {
                GameManager.instance.BlocoDestruido();
            }
        }
    }
}