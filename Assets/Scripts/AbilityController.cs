using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public NekoUI NekoUI;
    public AbilityHolder Holder;

    public int 剩余可用能力次数;
    public int 选中能力;

    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
    }

    private void Start()
    {
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
        if (剩余可用能力次数 > 0)
        {
            上升泡泡();
            固定泡泡();
        }
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
    }

    public void 上升泡泡()
    {
        if (选中能力 == (int)可用能力.上升泡泡 && Input.GetMouseButtonDown(0))
        {
            var localPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            localPos.z = 0;
            Transform a = Holder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(localPos);
            a.GetComponent<BubbleNormal>().enabled = true;
            a.GetComponent<SpriteRenderer>().enabled = true;
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
            a.GetComponent<BubbleFixed>().enabled = true;
            a.GetComponent<SpriteRenderer>().enabled = true;
            剩余可用能力次数--;
        }
    }
}
