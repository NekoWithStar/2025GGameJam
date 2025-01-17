using UnityEngine;

public class BubbleNormal : MonoBehaviour
{
    public float 上升速度 = 1f;
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
            // 每秒上升
            rb.velocity = new Vector2(0, 上升速度);
        }
    }

    public void Bomb()
    {
        Debug.Log("破碎");
        bomb = true;
        // TODO: 播动画
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
