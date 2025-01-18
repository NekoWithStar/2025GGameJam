using UnityEngine;
using System.Collections; 

public class BubbleChat : MonoBehaviour
{
    Animator animator;
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

    private float inputTimer = 1f;
    private bool inputDetected = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("bomb", false);
        rb = GetComponent<Rigidbody2D>();
        timer = lifeTime;
        currentLength = transform.localScale.x;
        yLength = transform.localScale.y;
        FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>().Follow = transform;
    }

    private void Update()
    {
        CheckBomb();
        CheckInput();

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
                float deltaScale = newScale - currentLength;
                transform.localScale = new Vector3(newScale, yLength, 1f);
                transform.position += new Vector3(deltaScale / 2, 0, 0); 
                currentLength = newScale;

                inputTimer = 1f;
                inputDetected = true;
            }
        }
        else if (inputCount >= maxInput && isEntering)
        {
            isEntering = false;
        }
    }

    private void CheckInput()
    {
        if (inputDetected)
        {
            inputTimer -= Time.deltaTime;
            if (inputTimer <= 0)
            {
                isEntering = false;
                inputDetected = false;
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
        animator.SetTrigger("BubbleChatBomb");
        animator.SetBool("bomb", true);
        Invoke("DestroyGameObject", 1f);
    }

    private void DestroyGameObject()
    {
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
