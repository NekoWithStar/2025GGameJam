using QFramework;
using QFramework.Example;
using UnityEngine;

public class BubbleTele : MonoBehaviour
{
    public Rigidbody2D rb;
    public BubbleTele pairBubble;
    public bool locked = false;
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

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && pairBubble != null && !locked)
        {
            locked = true;
            pairBubble.locked = true;
            FindFirstObjectByType<Player>().gameObject.transform.position = pairBubble.transform.position;
        }
    }
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && locked)
        {
            locked = false;
        }
    }
    public void Bomb()
    {
        AudioKit.PlaySound("Resources://Audios/Sounds/bubbledie");
        Destroy(gameObject);
    }
}
