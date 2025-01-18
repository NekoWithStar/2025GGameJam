using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public static bool CanPlace;

    public NekoUI NekoUI;
    public AbilityHolder Holder;

    public int 剩余可用能力次数;
    public int 选中能力;

    public float 最大允许距离;
    public bool 是否在范围内;
    public bool LockSwitch;
    private Player mPlayer;
    private Transform playerBody;

    private BubbleTele tempBubbleTele;

    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
    }

    private void Start()
    {
        mPlayer = FindObjectOfType<Player>();
        playerBody = mPlayer.Body.transform;
    }

    private void OnGUI()
    {
        if (选中能力 <= mLevelSettings.可用能力列表.Count - 1)
        {
            NekoUI.选中能力.text = "选中能力：" + AbilityBase.能力表[选中能力];
        }
    }

    public void Update()
    {
        修正选中能力();
        CheckDistance();
        if (剩余可用能力次数 > 0 && 是否在范围内)
        {
            上升泡泡();
            固定泡泡();
            聊天泡泡();
            传送泡泡();
        }
    }

    public void CheckDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        是否在范围内 = Vector3.Distance(mousePos, playerBody.position) < 最大允许距离;
    }

    public void 修正选中能力()
    {
        if (选中能力 < 0)
        {
            选中能力 = mLevelSettings.可用能力列表.Count - 1;
        }
        else if (选中能力 > mLevelSettings.可用能力列表.Count - 1)
        {
            选中能力 = 0;
        }
        if (LockSwitch)
        {
            选中能力 = (int)可用能力.传送泡泡;
        }
    }

    public void 上升泡泡()
    {
        if (选中能力 == (int)可用能力.上升泡泡 && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);
            剩余可用能力次数--;
        }
    }

    public void 固定泡泡()
    {
        if (选中能力 == (int)可用能力.固定泡泡 && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleFixed.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);
            剩余可用能力次数--;
        }
    }

    public void 聊天泡泡()
    {
        if (选中能力 == (int)可用能力.聊天泡泡 && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;

            Transform a = Holder.BubbleChat.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            if (!CheckCollision(a))
            {
                return;
            }
            a.transform.gameObject.SetActive(true);

            BubbleChat bubbleChat = a.GetComponent<BubbleChat>();
            bubbleChat.isRight = true;

            a.GetComponent<SpriteRenderer>().enabled = true;

            剩余可用能力次数--;
        }
    }

    public void 传送泡泡()
    {
        if (选中能力 == (int)可用能力.传送泡泡 && Input.GetMouseButtonDown(0))
        {
            if (!LockSwitch)
            {
                LockSwitch = true;
                Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                localPos.z = 0;
                Transform a = Holder.BubbleTele.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
                if (!CheckCollision(a))
                {
                    return;
                }
                tempBubbleTele = a.GetComponent<BubbleTele>();
                a.transform.gameObject.SetActive(true);
                剩余可用能力次数--;
            }
            else
            {
                LockSwitch = false;
                Vector3 localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                localPos.z = 0;
                Transform a = Holder.BubbleTele.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
                if (!CheckCollision(a))
                {
                    return;
                }
                a.transform.gameObject.SetActive(true);
                a.GetComponent<BubbleTele>().pairBubble = tempBubbleTele;
                tempBubbleTele.pairBubble = a.GetComponent<BubbleTele>();
                剩余可用能力次数--;
            }
        }
    }

    public bool CheckCollision(Transform bubble)
    {
        // 获取气泡的碰撞器
        Collider2D bubbleCollider = bubble.GetComponent<Collider2D>();

        // 检查气泡是否与其他物体发生碰撞
        Collider2D[] colliders = Physics2D.OverlapBoxAll(bubble.position, bubbleCollider.bounds.size, 0f);

        foreach (var collider in colliders)
        {
            Debug.Log(collider.name);
            // 如果碰撞到的不是玩家自身，则认为不能放置
            if ((!collider.CompareTag("Player") || !collider.CompareTag("Camare") && collider != bubbleCollider))
            {
                Destroy(bubble.gameObject);
                return false;
            }
        }

        return true;
    }
}
