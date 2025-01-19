using QFramework;
using QFramework.Example;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public static bool CanPlace = true;

    public AbilityHolder Holder;

    public int 剩余可用能力次数;
    public int listIndex;
    public int 选择能力key ;

    public float 最大允许距离;
    public static bool 是否在范围内;
    public bool LockSwitch;
    private Player mPlayer;
    private Transform playerBody;

    private BubbleTele tempBubbleTele;

    public Panel panel;
    public List<Sprite> unlocked_sprites;
    public List<Sprite> selected_sprites;
    public List<Image> panel_images;
    public Sprite Locked;

    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
        选择能力key = (int)mLevelSettings.可用能力列表[0];
    }

    private void Start()
    {
        AudioKit.Settings.IsSoundOn.Value = true;
        mPlayer = FindObjectOfType<Player>();
        playerBody = mPlayer.Body.transform;
    }

    public void Update()
    {
        修正选中能力();
        CheckDistance();
        UpdatedUI();
        panel.Count.text = "剩余泡泡 " + 剩余可用能力次数;
        选择能力key = (int)mLevelSettings.可用能力列表[listIndex];
        if (剩余可用能力次数 > 0 && 是否在范围内 && !InputManager.IsPaused && CanPlace)
        {
            上升泡泡();
            固定泡泡();
            聊天泡泡();
            传送泡泡();
        }
    }

    private void UpdatedUI()
    {
        for (int i = 0 ; i < 4; i++)
        {
            panel_images[i].sprite = Locked;
        }
        for (int i = 0; i < mLevelSettings.可用能力列表.Count; i++)
        {
            int gkey = (int)mLevelSettings.可用能力列表[i];
            panel_images[i].sprite = unlocked_sprites[gkey];
        }
        int mkey = (int)mLevelSettings.可用能力列表[listIndex];
        panel_images[listIndex].sprite = selected_sprites[mkey];
    }

    public void CheckDistance()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        是否在范围内 = Vector3.Distance(mousePos, playerBody.position) < 最大允许距离;
    }

    public void 修正选中能力()
    {
        if (listIndex < 0)
        {
            listIndex = mLevelSettings.可用能力列表.Count - 1;
        }
        else if (listIndex > mLevelSettings.可用能力列表.Count - 1)
        {
            listIndex = 0;
        }
        if (LockSwitch)
        {
            int index = mLevelSettings.可用能力列表.FindIndex(ability => (int)ability == 4);
            if (index != -1)
            {
                listIndex = index;
            }
        }
    }

    public void 上升泡泡()
    {
        if (选择能力key == (int)可用能力.上升泡泡 && Input.GetMouseButtonDown(0))
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
        if (选择能力key == (int)可用能力.固定泡泡 && Input.GetMouseButtonDown(0))
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
        if (选择能力key == (int)可用能力.聊天泡泡 && Input.GetMouseButtonDown(0))
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
        if (选择能力key == (int)可用能力.传送泡泡 && Input.GetMouseButtonDown(0))
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
            // 如果碰撞到的不是玩家自身，则认为不能放置
            if (!collider.gameObject.CompareTag("Player"))
            {
                //return false;
            }
        }
        return true;
    }
}
