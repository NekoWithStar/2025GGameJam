using UnityEngine;

public class BubbleNormal : MonoBehaviour
{
    public float �����ٶ� = 1f;
    public Rigidbody2D rb;


    private bool bomb = false;
    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
   
    private void FixedUpdate()
    {
        if(!bomb)
        {
            // ÿ������
            rb.velocity = new Vector2(0, �����ٶ�);
        }
    }

    public void Bomb()
    {
        Debug.Log("����");
        bomb = true;
        // TODO: ������
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Spike"))
        {
            Bomb();
        }
    }
}
