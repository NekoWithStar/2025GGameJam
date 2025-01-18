using UnityEngine;
using System.Collections;

public class BubbleFixed : MonoBehaviour
{
    public Rigidbody2D rb;
    public float lifeTime = 3f;
    private float timer = 3f;

    private bool bomb = false;

    private void Start()
    {
        timer = lifeTime;
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

    private void Update()
    {
        if (bomb) Bomb();
    }
    public void Bomb()
    {
        
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D otherRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                Debug.Log(otherRb.transform.name);
                Vector2 savedVelocity = otherRb.velocity;
                RigidbodyConstraints2D savedConstraints = otherRb.constraints;
                otherRb.velocity = Vector2.zero;
                otherRb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(ResetRigidbody(otherRb, savedVelocity, savedConstraints));
            }
            }
        }

    private IEnumerator ResetRigidbody(Rigidbody2D otherRb, Vector2 savedVelocity, RigidbodyConstraints2D saveConstraints)
    {
        bomb = false;
        timer = lifeTime + 1f;
        yield return new WaitForSeconds(lifeTime);
        otherRb.constraints = saveConstraints;
        otherRb.velocity = savedVelocity;
        bomb = true;
    }
}
