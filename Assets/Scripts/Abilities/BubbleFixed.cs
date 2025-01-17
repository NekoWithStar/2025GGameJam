using UnityEngine;
using System.Collections;

public class BubbleFixed : MonoBehaviour
{
    public Rigidbody2D rb;
    private float timer = 3f;

    private bool bomb = false;
    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        enabled = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!bomb)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                Bomb();
            }
        }
    }

    public void Bomb()
    {
        Debug.Log("破碎");
        bomb = true;
        StartCoroutine(BombAndDestroy());
    }

    private IEnumerator BombAndDestroy()
    {
        // 在这里可以添加爆炸效果的代码
        yield return new WaitForSeconds(0.1f); 
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("碰撞");
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 savedVelocity = otherRb.velocity;
            otherRb.velocity = Vector2.zero;
            otherRb.simulated = false;
            StartCoroutine(ResetRigidbody(otherRb, savedVelocity));
        }
    }

    private IEnumerator ResetRigidbody(Rigidbody2D otherRb, Vector2 savedVelocity)
    {
        yield return new WaitForSeconds(0.2f); 
        otherRb.simulated = true;
        otherRb.velocity = savedVelocity;
    }
}
