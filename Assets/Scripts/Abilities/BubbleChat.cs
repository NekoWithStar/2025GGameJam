using UnityEngine;

public class BubbleChat : MonoBehaviour
{
    public Rigidbody2D rb;
    public float lifeTime = 3f;
    private float timer = 3f;
    public Vector2 direction;
    private bool isEntering = true;

    public int maxInput = 100; 
    public float maxLength = 5f; 
    private float currentLength = 0f; 
    private int inputCount = 0; 

    private Transform bubbleTransform; 

    private bool bomb = false;

    private void Awake()
    {
        enabled = false;
        bubbleTransform = transform.Find("BubbleChat");
    }

    private void Start()
    {
        enabled = true;
        rb = GetComponent<Rigidbody2D>();
        timer = lifeTime;
    }

    private void Update()
    {
        if (inputCount < maxInput && isEntering) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                isEntering = false;
            } 
            if (Input.anyKeyDown) {
                inputCount++;
                float newScale = Mathf.Min(currentLength + 0.1f, maxLength); 
                bubbleTransform.localScale = new Vector3(newScale, newScale, 1f); 
                currentLength = newScale;
            }
        }
        if (!bomb && !isEntering)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Bomb();
            }
        }
    }

    public void Bomb()
    {
        Debug.Log("ÆÆËé");
        bomb = true;
        // TODO: ²¥¶¯»­
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Bomb();
        }
    }
}
