using UnityEngine;

public class BubbleNormal : MonoBehaviour
{
    public float �����ٶ� = 1f;
    public Rigidbody2D rb;
    public float lifeTime = 5f;
    private float leftTime;


    private bool bomb = false;
    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftTime = lifeTime;
    }
   
    private void FixedUpdate()
    {
        if(!bomb)
        {
            // ÿ������
            rb.velocity = new Vector2(0, �����ٶ�);
        }
    }

    private void Update()
    {
        if (!bomb)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                Bomb();
            }
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
