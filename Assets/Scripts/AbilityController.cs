using QFramework;
using QFramework.Example;
using UnityEngine;

public class AbilityController : ViewController
{
    public LevelSettings mLevelSettings;

    public NekoUI NekoUI;
    public AbilityHolder Holder;

    public int ʣ�������������;
    public int ѡ������;



    public void Awake()
    {
        ʣ������������� = mLevelSettings.�ܿ�����������;
    }

    private void Start()
    {
        var a = Holder.BubbleNormal.InstantiateWithParent(transform.Find("Root")).LocalPosition(new Vector3(0, 2));
        a.GetComponent<BubbleNormal>().enabled = true;
        a.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnGUI()
    {
        NekoUI.ѡ������.text = "ѡ��������" + AbilityBase.������[ѡ������];
    }
    public void Update()
    {
        
    }
}
