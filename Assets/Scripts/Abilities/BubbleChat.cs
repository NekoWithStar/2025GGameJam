using UnityEngine;
using System.Collections;
using QFramework;
using QFramework.Example;

public class BubbleChat : MonoBehaviour
{
    Animator animator;
    public Rigidbody2D rb;
    public float lifeTime = 3f;
    private float timer = 3f;
    public bool isRight = true;
    [SerializeField]
    private bool isEntering = true;

    public int maxInput = 100;
    public float maxLength = 10f;
    public float deltaLength = 0.2f;
    private float currentLength = 0f;
    private float yLength = 0f;
    private int inputCount = 0;

    private bool bomb = false;

    private float inputTimer = 1f;
    private bool inputDetected = false;

    private Cinemachine.CinemachineVirtualCamera cmr;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("bomb", false);
        rb = GetComponent<Rigidbody2D>();
        timer = lifeTime;
        currentLength = transform.localScale.x;
        yLength = transform.localScale.y;
        cmr = FindFirstObjectByType<Cinemachine.CinemachineVirtualCamera>();
        cmr.Follow = transform;
    }

    private void Update()
    {
        CheckBomb();
        CheckInput();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isEntering = false;
            cmr.Follow = FindFirstObjectByType<Player>().transform;
            cmr.LookAt= FindFirstObjectByType<Player>().transform;
        }

        if (inputCount < maxInput && isEntering)
        {
            if (Input.anyKeyDown)
            {
                inputCount++;
                float newScale = Mathf.Min(currentLength +deltaLength,  maxLength);
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
        cmr.Follow = FindFirstObjectByType<Player>().transform;
        cmr.LookAt = FindFirstObjectByType<Player>().transform;
        animator.SetBool("bomb", true);
        AudioKit.PlaySound("Resources://Audios/Sounds/bubbledie");
        Invoke(nameof(DestroyGameObject), 1.5f);
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
