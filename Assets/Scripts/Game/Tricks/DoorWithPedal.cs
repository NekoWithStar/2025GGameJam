using UnityEngine;

namespace QFramework.Example
{
    public partial class DoorWithPedal : ViewController
    {
        public bool canMove = true;
        public float upTime = 0.5f; // 上升时间
        public float distance = 5f; // 最高高度

        private bool isMovingUp = false;
        private bool isMovingDown = false;
        private float timer = 0f;

        public Rigidbody2D rb;
        private Vector2 initialPosition;

        private void Awake()
        {
            rb = Door.GetComponent<Rigidbody2D>();
            initialPosition = rb.position;
        }

        void Start()
        {
            Pedal.OnTriggerEnter2DEvent(e =>
            {
                isMovingUp = true;
                isMovingDown = false;
                timer = 0f;
            });

            Pedal.OnTriggerExit2DEvent(e =>
            {
                isMovingDown = true;
                isMovingUp = false;
                timer = 0f;
            });
        }

        private void FixedUpdate()
        {
            if (canMove)
            {
                if (isMovingUp)
                {
                    MoveUp();
                }
                else if (isMovingDown)
                {
                    MoveDown();
                }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void MoveUp()
        {
            timer += Time.deltaTime;
            float progress = timer / upTime;
            Vector2 targetPosition = initialPosition + Vector2.up * distance;
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.velocity = direction * (distance / upTime);

            if (progress >= 1f)
            {
                isMovingUp = false;
                rb.velocity = Vector2.zero;
            }
        }

        private void MoveDown()
        {
            timer += Time.deltaTime;
            float progress = timer / upTime;
            Vector2 targetPosition = initialPosition;
            Vector2 direction = (targetPosition - rb.position).normalized;
            rb.velocity = direction * (distance / upTime);

            if (progress >= 1f)
            {
                isMovingDown = false;
                rb.velocity = Vector2.zero;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bubble"))
            {
                canMove = false;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Bubble"))
            {
                canMove = true;
            }
        }
    }
}
