using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public NekoUI NekoUI;
    public AbilityHolder AblilityHolder;

    public int 剩余可用能力次数;
    public int 选中能力;



    public void Awake()
    {
        剩余可用能力次数 = mLevelSettings.总可用能力次数;
    }

    private void Start()
    {
        var a = AblilityHolder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(new Vector3(0, 2));
        a.GetComponent<BubbleNormal>().enabled = true;
        a.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnGUI()
    {
        NekoUI.选中能力.text = "选中能力：" + AbilityBase.能力表[选中能力];
    }
    public void Update()
    {
        
    }
}
