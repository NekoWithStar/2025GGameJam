using UnityEngine;

public class BubbleChat : MonoBehaviour
{
    public Rigidbody2D rb;
    public float lifeTime = 3f;
    private float timer = 3f;
    public bool isRight = true;
    private bool isEntering = true;

    public int maxInput = 100; 
    public float maxLength = 10f; 
    private float currentLength = 0f;
    private float yLength = 0f;
    private int inputCount = 0; 

    private bool bomb = false;

    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = lifeTime;
        currentLength = transform.localScale.x;
        yLength = transform.localScale.y;
        FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    }

    private void Update()
    {
        CheckBomb();

        if (inputCount < maxInput && isEntering)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isEntering = false;
                FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>().Follow = FindFirstObjectByType<ÒÆ¶¯>().transform;
            }
            if (Input.anyKeyDown)
            {
                inputCount++;
                float newScale = Mathf.Min(currentLength + 0.2f, maxLength);
                transform.localScale = new Vector3(newScale, yLength, 1f);
                currentLength = newScale;
            }
        }
    }

    private void CheckBomb()
    {
        if (!bomb && !isEntering)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                bomb = true;
            }
        }
        else if (bomb && !isEntering)
        {
            Bomb();
        }
    }

    public void Bomb()
    {
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
