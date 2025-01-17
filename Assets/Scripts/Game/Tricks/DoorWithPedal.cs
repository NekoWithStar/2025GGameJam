using UnityEngine;

namespace QFramework.Example
{
    public partial class DoorWithPedal : ViewController
    {
        public bool canMove = true;
        public float moveTime = 0.5f; // 移动时间
        public float distance = 5f;   // 移动距离

        [SerializeField]
        private bool isMovingUp = false;
        [SerializeField]
        private bool isMovingDown = false;

        public Rigidbody2D rb;
        private Vector2 initialPosition;
        private Vector2 targetPosition;

        private void Awake()
        {
            rb = Door.GetComponent<Rigidbody2D>();
            initialPosition = rb.position;  // 记录初始位置
            targetPosition = initialPosition + Vector2.up * distance; // 计算目标位置
        }

        void Start()
        {
            Pedal.OnTriggerEnter2DEvent(e =>
            {
                if (canMove)
                {
                    isMovingUp = true;
                    isMovingDown = false;
                }
            });

            Pedal.OnTriggerExit2DEvent(e =>
            {
                if (canMove)
                {
                    isMovingDown = true;
                    isMovingUp = false;
                }
            });

            Door.OnCollisionEnter2DEvent(c =>
            {
                if (c.gameObject.CompareTag("Bubble"))
                {
                    canMove = false;
                    rb.velocity = Vector2.zero; // 停止移动
                }
            });

            Door.OnCollisionExit2DEvent(c =>
            {
                if (c.gameObject.CompareTag("Bubble"))
                {
                    canMove = true;
                }
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
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else
            {
                rb.velocity = Vector2.zero; // 停止移动
            }

            // 检测并纠正位置偏差，防止穿模
            CheckPosition();
        }

        private void MoveUp()
        {
            Vector2 direction = Vector2.up;
            rb.velocity = direction * (distance / moveTime);

            // 当达到目标位置时停止移动
            if (rb.position.y >= targetPosition.y)
            {
                rb.position = targetPosition;
                rb.velocity = Vector2.zero;
                isMovingUp = false;
            }
        }

        private void MoveDown()
        {
            Vector2 direction = Vector2.down;
            rb.velocity = direction * (distance / moveTime);

            // 当达到初始位置时停止移动
            if (rb.position.y <= initialPosition.y)
            {
                rb.position = initialPosition;
                rb.velocity = Vector2.zero;
                isMovingDown = false;
            }
        }

        private void CheckPosition()
        {
            // 定义允许的最大偏移范围
            float maxOffset = distance * 1.1f;

            // 如果位置超出范围，重置位置和速度
            if (Mathf.Abs(rb.position.y - initialPosition.y) > maxOffset)
            {
                rb.position = isMovingUp ? targetPosition : initialPosition;
                rb.velocity = Vector2.zero;
                isMovingUp = false;
                isMovingDown = false;
            }
        }
    }
}