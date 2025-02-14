using QFramework;
using UnityEngine;

public class BubbleNormal : MonoBehaviour
{
    Animator animator;
    public float 上升速度 = 1f;
    public float 水平速度 = 0f;
    public Rigidbody2D rb;
    public float lifeTime = 5f;
    private float leftTime;


    private bool bomb = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        leftTime = lifeTime;
        animator.SetBool("bomb", false);
    }
   
    private void FixedUpdate()
    {
        if(!bomb)
        {
            // 每秒上升
            rb.velocity = new Vector2(水平速度, 上升速度);
        }
    }

    private void Update()
    {
        if (!bomb)
        {
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                bomb = true;
            }
        }
        else
        {
            Bomb();
        }
    }

    public void Bomb()
    {
        animator.SetBool("bomb", true);
        AudioKit.PlaySound("Resources://Audios/Sounds/bubbledie");
        Invoke(nameof(DestroyGameObject), 1f);
    }

    private void DestroyGameObject()
    {
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
